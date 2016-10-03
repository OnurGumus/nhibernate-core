#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1922
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var joe = new Customer()
					{ValidUntil = new DateTime(2000, 1, 1)};
					await (session.SaveAsync(joe));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Customer"));
					await (tx.CommitAsync());
				}
			}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task CanExecuteQueryOnStatelessSessionUsingDetachedCriteriaAsync()
		{
			using (var stateless = sessions.OpenStatelessSession())
			{
				var dc = DetachedCriteria.For<Customer>().Add(Restrictions.Eq("ValidUntil", new DateTime(2000, 1, 1)));
				var cust = await (dc.GetExecutableCriteria(stateless).UniqueResultAsync());
				Assert.IsNotNull(cust);
			}
		}
	}
}
#endif
