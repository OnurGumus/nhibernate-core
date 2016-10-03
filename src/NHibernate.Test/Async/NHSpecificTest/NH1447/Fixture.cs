#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1447
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var e1 = new Person("Tuna Toksoz", false);
					var e2 = new Person("Oguz Kurumlu", true);
					await (s.SaveAsync(e1));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanQueryByConstantProjectionWithTypeAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria c = s.CreateCriteria(typeof (Person)).Add(Restrictions.EqProperty("WantsNewsletter", Projections.Constant(false, NHibernateUtil.Boolean)));
				IList<Person> list = await (c.ListAsync<Person>());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
