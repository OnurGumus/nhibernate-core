#if NET_4_5
using System.Data;
using log4net;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1810
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
	}
}
#endif
