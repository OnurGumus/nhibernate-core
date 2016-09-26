#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.Pagination
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomDialectFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Pagination.DataPoint.hbm.xml"};
			}
		}

		protected override void Configure(Configuration configuration)
		{
			if (!(Dialect is Dialect.MsSql2005Dialect))
				Assert.Ignore("Test is for SQL dialect only");
			cfg.SetProperty(Environment.Dialect, typeof (CustomMsSqlDialect).AssemblyQualifiedName);
			cfg.SetProperty(Environment.ConnectionDriver, typeof (CustomMsSqlDriver).AssemblyQualifiedName);
		}

		private CustomMsSqlDialect CustomDialect
		{
			get
			{
				return (CustomMsSqlDialect)Sfi.Dialect;
			}
		}

		private CustomMsSqlDriver CustomDriver
		{
			get
			{
				return (CustomMsSqlDriver)Sfi.ConnectionProvider.Driver;
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			CustomDriver.CustomMsSqlDialect = CustomDialect;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new DataPoint()
					{X = 5}));
					await (s.SaveAsync(new DataPoint()
					{X = 6}));
					await (s.SaveAsync(new DataPoint()
					{X = 7}));
					await (s.SaveAsync(new DataPoint()
					{X = 8}));
					await (s.SaveAsync(new DataPoint()
					{X = 9}));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from DataPoint"));
					await (t.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task LimitFirstAsync()
		{
			using (ISession s = OpenSession())
			{
				CustomDialect.ForcedSupportsVariableLimit = true;
				CustomDialect.ForcedBindLimitParameterFirst = true;
				var points = await (s.CreateCriteria<DataPoint>().Add(Restrictions.Gt("X", 5.1d)).AddOrder(Order.Asc("X")).SetFirstResult(1).SetMaxResults(2).ListAsync<DataPoint>());
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points[0].X, Is.EqualTo(7d));
				Assert.That(points[1].X, Is.EqualTo(8d));
			}
		}

		[Test]
		public async Task LimitFirstMultiCriteriaAsync()
		{
			using (ISession s = OpenSession())
			{
				CustomDialect.ForcedSupportsVariableLimit = true;
				CustomDialect.ForcedBindLimitParameterFirst = true;
				var criteria = s.CreateMultiCriteria().Add<DataPoint>(s.CreateCriteria<DataPoint>().Add(Restrictions.Gt("X", 5.1d)).AddOrder(Order.Asc("X")).SetFirstResult(1).SetMaxResults(2));
				var points = (IList<DataPoint>)(await (criteria.ListAsync()))[0];
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points[0].X, Is.EqualTo(7d));
				Assert.That(points[1].X, Is.EqualTo(8d));
			}
		}
	}
}
#endif
