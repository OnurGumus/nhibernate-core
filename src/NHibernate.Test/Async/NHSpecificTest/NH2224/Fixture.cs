#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2224
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is NHibernate.Dialect.SQLiteDialect;
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var class1 = new Class1()
					{DateOfChange = DateTime.Now};
					await (s.SaveAsync(class1));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Class1"));
					await (t.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task CanQueryBasedOnYearWithInOperatorAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var criteria = s.CreateCriteria<Class1>();
					criteria.Add(Restrictions.In(Projections.SqlFunction("year", NHibernateUtil.DateTime, Projections.Property("DateOfChange")), new string[]{"2010", DateTime.Now.Year.ToString()}));
					var result = await (criteria.ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
