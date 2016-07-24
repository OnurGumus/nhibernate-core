#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1297
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1297";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Model"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task ItemsCanBeSavedAndUpdatedInTheSameSessionAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Model m = new Model();
					m.Name = "model";
					m.Items.Add(new Item()); // New Item added here
					await (s.SaveAsync(m));
					await (s.FlushAsync()); // Push changes to database; otherwise, bug does not manifest itself
					m.Items[0].Name = "new Name"; // Same new item updated here
					await (tx.CommitAsync()); // InvalidCastException would be thrown here before the bug fix
				}
		}
	}
}
#endif
