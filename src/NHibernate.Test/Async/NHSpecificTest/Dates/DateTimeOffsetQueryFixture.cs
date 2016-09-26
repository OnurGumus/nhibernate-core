#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Linq;
using NHibernate.Driver;
using NHibernate.Type;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeOffsetQueryFixtureAsync : FixtureBaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.Dates.Mappings.DateTimeOffset.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			// Cannot handle DbType.DateTimeOffset via ODBC.
			if (factory.ConnectionProvider.Driver is OdbcDriver)
				return false;
			return base.AppliesTo(factory);
		}

		protected override DbType? AppliesTo()
		{
			return DbType.DateTimeOffset;
		}

		protected override void Configure(Cfg.Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.ShowSql, "true");
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			var dates1 = new AllDates{Sql_datetimeoffset = new DateTimeOffset(2012, 11, 1, 1, 0, 0, TimeSpan.FromHours(1))};
			var dates2 = new AllDates{Sql_datetimeoffset = new DateTimeOffset(2012, 11, 1, 2, 0, 0, TimeSpan.FromHours(3))};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(dates1));
					await (s.SaveAsync(dates2));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from AllDates"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanQueryWithCastInHqlAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("select cast(min(Sql_datetimeoffset), datetimeoffset) from AllDates").UniqueResultAsync<DateTimeOffset>());
					Assert.That(datesRecovered, Is.EqualTo(new DateTimeOffset(2012, 11, 1, 2, 0, 0, TimeSpan.FromHours(3))));
				}
		}
	}
}
#endif
