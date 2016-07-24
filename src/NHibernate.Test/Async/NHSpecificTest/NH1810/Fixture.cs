#if NET_4_5
using System.Data;
using log4net;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1810
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		// The problem is the same using a default sort="natural" collection for Children
		// and there is no problem using a default HashSet.
		// look a the implementation of Children
		private static readonly ILog Log = LogManager.GetLogger(typeof (FixtureAsync));
		int parentId;
		int doctorId;
		protected override ISession OpenSession()
		{
			var session = base.OpenSession();
			session.FlushMode = FlushMode.Commit;
			return session;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					var parent = new Parent{Address = "A street, A town, A country"};
					// If you add a child all work fine.
					//var child = new Child {Age = 2, Parent = parent};
					//parent.Children.AddChild(child);
					await (sess.SaveAsync(parent));
					var doctor = new Doctor{DoctorNumber = 123, MedicalRecord = parent.MedicalRecord};
					await (sess.SaveAsync(doctor));
					await (tx.CommitAsync());
					parentId = parent.Id;
					doctorId = doctor.Id;
				}
		}

		[Test]
		public async Task TestAsync()
		{
			Log.Debug("Entering test");
			using (ISession sess = OpenSession())
			{
				Log.Debug("Loading doctor");
				var doctor = await (sess.GetAsync<Doctor>(doctorId)); // creates a proxy of the medical record
				Log.Debug("Loading parent");
				var parent = await (sess.GetAsync<Parent>(parentId));
				Log.Debug("Adding new child to parent");
				parent.Children.AddChild(new Child{Age = 10, Parent = parent}); // does NOT cause Child.GetHashCode() to be called
				using (ITransaction tx = sess.BeginTransaction(IsolationLevel.ReadCommitted))
				{
					Log.Debug("Saving parent");
					await (sess.UpdateAsync(parent));
					Log.Debug("Committing transaction");
					await (tx.CommitAsync()); // triggers Child.GetHashCode() to be called in flush machiney, leading to CNPBF exception
				}
			}

			Log.Debug("Exiting test");
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from Doctor"));
					await (sess.DeleteAsync("from Parent"));
					await (sess.DeleteAsync("from Child"));
					await (sess.DeleteAsync("from MedicalRecord"));
					await (sess.DeleteAsync("from Disease"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
