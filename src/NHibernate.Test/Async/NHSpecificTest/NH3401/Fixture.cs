#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3401
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test(Description = "NH-3401")]
		[Ignore("Test not implemented - this can be used a base for a proper test case for NH-3401.")]
		public async Task YesNoParameterLengthShouldBe1Async()
		{
			// MISSING PART: Asserts for the SQL parameter sizes in the generated commands.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally", YesNo = true};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result =
						from e in session.Query<Entity>()where e.YesNo
						select e;
					Assert.AreEqual(1, result.ToList().Count);
				}
		}
	}
}
#endif
