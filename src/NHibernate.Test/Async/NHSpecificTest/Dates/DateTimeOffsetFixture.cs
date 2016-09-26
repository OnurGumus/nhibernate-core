#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using NHibernate.Driver;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeOffsetFixtureAsync : FixtureBaseAsync
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

		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			DateTimeOffset NowOS = DateTimeOffset.Now;
			AllDates dates = new AllDates{Sql_datetimeoffset = NowOS};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(dates));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("from AllDates").UniqueResultAsync<AllDates>());
					Assert.That(datesRecovered.Sql_datetimeoffset, Is.EqualTo(NowOS));
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("from AllDates").UniqueResultAsync<AllDates>());
					await (s.DeleteAsync(datesRecovered));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task HashCodeShouldHaveSameBehaviorOfNetTypeAsync()
		{
			var type = new DateTimeOffsetType();
			var now = DateTimeOffset.Now;
			var exactClone = new DateTimeOffset(now.Ticks, now.Offset);
			Assert.That((now.GetHashCode() == exactClone.GetHashCode()), Is.EqualTo(now.GetHashCode() == await (type.GetHashCodeAsync(exactClone, EntityMode.Poco))));
		}

		[Test]
		public async Task NextAsync()
		{
			var type = NHibernateUtil.DateTimeOffset;
			var current = DateTimeOffset.Now.AddTicks(-1);
			object next = await (type.NextAsync(current, null));
			Assert.That(next, Is.TypeOf<DateTimeOffset>().And.Property("Ticks").GreaterThan(current.Ticks));
		}

		[Test]
		public async Task SeedAsync()
		{
			var type = NHibernateUtil.DateTimeOffset;
			Assert.That(await (type.SeedAsync(null)), Is.TypeOf<DateTimeOffset>());
		}
	}
}
#endif
