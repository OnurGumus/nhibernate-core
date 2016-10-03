#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2907
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task ShouldNotEagerLoadKeyManyToOneWhenOverridingGetHashCodeAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var grp = new Group();
					await (s.SaveAsync(grp));
					var loanId = new Dictionary<string, object>{{"Id", 1}, {"Group", grp}};
					var loan = new Dictionary<string, object>{{"CompId", loanId}, {"Name", "money!!!"}};
					await (s.SaveAsync("Loan", loan));
					await (tx.CommitAsync());
				}

			bool isInitialized;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var loan = await (s.CreateQuery("select l from Loan l").UniqueResultAsync<IDictionary>());
					var compId = (IDictionary)loan["CompId"];
					var group = compId["Group"];
					Assert.That(@group, Is.Not.Null);
					isInitialized = NHibernateUtil.IsInitialized(group);
					await (tx.CommitAsync());
				}

			Assert.That(isInitialized, Is.False);
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
