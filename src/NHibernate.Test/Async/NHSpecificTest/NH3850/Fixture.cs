﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.SqlTypes;
using NHibernate.Util;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH3850
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		private const string _searchName1 = "name";
		private const string _searchName2 = "name2";
		private const int _totalEntityCount = 10;
		private readonly DateTime _testDate = DateTime.Now;
		private readonly DateTimeOffset _testDateWithOffset = DateTimeOffset.Now;

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return TestDialect.SupportsSqlType(new SqlType(DbType.DateTimeOffset));
		}

		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			// Cannot handle DbType.DateTimeOffset via ODBC.
			return !(factory.ConnectionProvider.Driver is OdbcDriver);
		}

		protected override void OnSetUp()
		{
			base.OnSetUp();
			using (var session = OpenSession())
			{
				var dateTime1 = _testDate.AddDays(-1);
				var dateTime2 = _testDate.AddDays(1);
				var dateTimeOffset1 = _testDateWithOffset.AddDays(-1);
				var dateTimeOffset2 = _testDateWithOffset.AddDays(1);
				Action<DomainClassBase> init1 = dc =>
				{
					dc.Id = 1;
					dc.Name = _searchName1;
					dc.Integer = 1;
					dc.Long = 1;
					dc.Decimal = 1;
					dc.Double = 1;
					dc.DateTime = dateTime1;
					dc.DateTimeOffset = dateTimeOffset1;
					dc.NonNullableDecimal = 1;
				};
				Action<DomainClassBase> init2 = dc =>
				{
					dc.Id = 2;
					dc.Name = _searchName2;
					dc.Integer = 2;
					dc.Long = 2;
					dc.Decimal = 2;
					dc.Double = 2;
					dc.DateTime = dateTime2;
					dc.DateTimeOffset = dateTimeOffset2;
					dc.NonNullableDecimal = 2;
				};

				DomainClassBase entity = new DomainClassBExtendedByA();
				init1(entity);
				session.Save(entity);
				entity = new DomainClassBExtendedByA();
				init2(entity);
				session.Save(entity);

				entity = new DomainClassCExtendedByD();
				init1(entity);
				session.Save(entity);
				entity = new DomainClassCExtendedByD();
				init2(entity);
				session.Save(entity);

				entity = new DomainClassE();
				init1(entity);
				session.Save(entity);
				entity = new DomainClassE();
				init2(entity);
				session.Save(entity);

				entity = new DomainClassGExtendedByH();
				init1(entity);
				session.Save(entity);
				entity = new DomainClassGExtendedByH();
				init2(entity);
				session.Save(entity);
				entity = new DomainClassHExtendingG
				{
					Id = 3,
					Name = _searchName1,
					Integer = 3,
					Long = 3,
					Decimal = 3,
					Double = 3,
					DateTime = dateTime1,
					DateTimeOffset = dateTimeOffset1,
					NonNullableDecimal = 3
				};
				session.Save(entity);
				entity = new DomainClassHExtendingG
				{
					Id = 4,
					Name = _searchName2,
					Integer = 4,
					Long = 4,
					Decimal = 4,
					Double = 4,
					DateTime = dateTime2,
					DateTimeOffset = dateTimeOffset2,
					NonNullableDecimal = 4
				};
				session.Save(entity);

				session.Flush();
			}
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();
			using (var session = OpenSession())
			{
				var hql = "from System.Object";
				session.Delete(hql);
				session.Flush();
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task AnyBBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().AnyAsync());
				Assert.IsTrue(result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task AnyBBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().AnyAsync(dc => dc.Name == _searchName1));
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyCBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().AnyAsync());
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyCBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().AnyAsync(dc => dc.Name == _searchName1));
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyEAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().AnyAsync());
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyEWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().AnyAsync(dc => dc.Name == _searchName1));
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyFAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().AnyAsync());
				Assert.IsFalse(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyFWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().AnyAsync(dc => dc.Name == _searchName1));
				Assert.IsFalse(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyGBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().AnyAsync());
				Assert.IsTrue(result);
			}
		}

		// Non-reg case
		[Test]
		public async Task AnyGBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().AnyAsync(dc => dc.Name == _searchName1));
				Assert.IsTrue(result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task AnyObjectAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<object>().AnyAsync());
				Assert.IsTrue(result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test, Ignore("Won't fix: requires reshaping the query")]
		public Task AverageBBaseAsync()
		{
			return AverageAsync<DomainClassBExtendedByA>(1.5m);
		}

		// Failing case till NH-3850 is fixed
		[Test, Ignore("Won't fix: requires reshaping the query")]
		public Task AverageCBaseAsync()
		{
			return AverageAsync<DomainClassCExtendedByD>(1.5m);
		}

		// Non-reg case
		[Test]
		public Task AverageEAsync()
		{
			return AverageAsync<DomainClassE>(1.5m);
		}

		// Non-reg case
		[Test]
		public Task AverageFAsync()
		{
			return AverageAsync<DomainClassF>(null);
		}

		// Failing case till NH-3850 is fixed
		[Test, Ignore("Won't fix: requires reshaping the query")]
		public Task AverageGBaseAsync()
		{
			return AverageAsync<DomainClassGExtendedByH>(2.5m);
		}

		private async Task AverageAsync<DC>(decimal? expectedResult, CancellationToken cancellationToken = default(CancellationToken)) where DC : DomainClassBase
		{
			using (var session = OpenSession())
			{
				var dcQuery = session.Query<DC>();
				var integ = await (dcQuery.AverageAsync(dc => dc.Integer, cancellationToken));
				Assert.AreEqual(expectedResult, integ, "Integer average has failed");
				var longInt = await (dcQuery.AverageAsync(dc => dc.Long, cancellationToken));
				Assert.AreEqual(expectedResult, longInt, "Long integer average has failed");
				var dec = await (dcQuery.AverageAsync(dc => dc.Decimal, cancellationToken));
				Assert.AreEqual(expectedResult, dec, "Decimal average has failed");
				var dbl = await (dcQuery.AverageAsync(dc => dc.Double, cancellationToken));
				Assert.AreEqual(expectedResult, dbl, "Double average has failed");

				if (expectedResult.HasValue)
				{
					var nonNullableDecimal = -1m;
					Assert.DoesNotThrowAsync(async () => { nonNullableDecimal = await (dcQuery.AverageAsync(dc => dc.NonNullableDecimal, cancellationToken)); }, "Non nullable decimal average has failed");
					Assert.AreEqual(expectedResult, nonNullableDecimal, "Non nullable decimal average has failed");
				}
				else
				{
					Assert.That(() => { dcQuery.Average(dc => dc.NonNullableDecimal); },
						// After fix
						Throws.InstanceOf<InvalidOperationException>()
						// Before fix
						.Or.InnerException.InstanceOf<ArgumentNullException>(),
						"Non nullable decimal average has failed");
				}
			}
		}

		// Failing case till NH-3850 is fixed
		[Test, Ignore("Won't fix: requires reshaping the query")]
		public async Task AverageObjectAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<object>().AverageAsync(o => (int?)2));
				Assert.AreEqual(2, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task CountBBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().CountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task CountBBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().CountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountCBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().CountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountCBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().CountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountEAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().CountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountEWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().CountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountFAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().CountAsync());
				Assert.AreEqual(0, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task CountFWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().CountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(0, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task CountGBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().CountAsync());
				Assert.AreEqual(4, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task CountGBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().CountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(2, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task CountObjectAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<object>().CountAsync());
				Assert.AreEqual(_totalEntityCount, result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultBBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassBExtendedByA>();
				DomainClassBExtendedByA result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNotNull(result);
				Assert.IsInstanceOf<DomainClassBExtendedByA>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultBBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassBExtendedByA>();
				DomainClassBExtendedByA result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassBExtendedByA>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultCBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassCExtendedByD>();
				DomainClassCExtendedByD result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNotNull(result);
				Assert.IsInstanceOf<DomainClassCExtendedByD>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultCBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassCExtendedByD>();
				DomainClassCExtendedByD result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassCExtendedByD>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultEAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassE>();
				DomainClassE result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNotNull(result);
				Assert.IsInstanceOf<DomainClassE>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultEWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassE>();
				DomainClassE result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassE>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultFAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassF>();
				DomainClassF result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNull(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultFWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassF>();
				DomainClassF result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNull(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultGBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassGExtendedByH>();
				DomainClassGExtendedByH result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNotNull(result);
				// If class type assert starts failing, maybe just ignore it: order of first on polymorphic queries looks unspecified to me.
				Assert.IsInstanceOf<DomainClassGExtendedByH>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultGBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassGExtendedByH>();
				DomainClassGExtendedByH result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				// If class type assert starts failing, maybe just ignore it: order of first on polymorphic queries looks unspecified to me.
				Assert.IsInstanceOf<DomainClassGExtendedByH>(result);
			}
		}

		// Non-reg case
		[Test]
		public void FirstOrDefaultObjectAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<object>();
				object result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.FirstOrDefaultAsync()); });
				Assert.IsNotNull(result);
				// If class type assert starts failing, maybe just ignore it: order of first on polymorphic queries looks unspecified to me.
				Assert.IsInstanceOf<DomainClassBExtendedByA>(result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task LongCountBBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().LongCountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task LongCountBBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassBExtendedByA>().LongCountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountCBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().LongCountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountCBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassCExtendedByD>().LongCountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountEAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().LongCountAsync());
				Assert.AreEqual(2, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountEWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassE>().LongCountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(1, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountFAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().LongCountAsync());
				Assert.AreEqual(0, result);
			}
		}

		// Non-reg case
		[Test]
		public async Task LongCountFWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassF>().LongCountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(0, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task LongCountGBaseAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().LongCountAsync());
				Assert.AreEqual(4, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task LongCountGBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<DomainClassGExtendedByH>().LongCountAsync(dc => dc.Name == _searchName1));
				Assert.AreEqual(2, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task LongCountObjectAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<object>().LongCountAsync());
				Assert.AreEqual(_totalEntityCount, result);
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task MaxBBaseAsync()
		{
			return MaxAsync<DomainClassBExtendedByA>(2);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task MaxCBaseAsync()
		{
			return MaxAsync<DomainClassCExtendedByD>(2);
		}

		// Non-reg case
		[Test]
		public Task MaxEAsync()
		{
			return MaxAsync<DomainClassE>(2);
		}

		// Non-reg case
		[Test]
		public Task MaxFAsync()
		{
			return MaxAsync<DomainClassF>(null);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task MaxGBaseAsync()
		{
			return MaxAsync<DomainClassGExtendedByH>(4);
		}

		private async Task MaxAsync<DC>(int? expectedResult, CancellationToken cancellationToken = default(CancellationToken)) where DC : DomainClassBase
		{
			using (var session = OpenSession())
			{
				var dcQuery = session.Query<DC>();
				var name = await (dcQuery.MaxAsync(dc => dc.Name, cancellationToken));
				Assert.AreEqual(expectedResult.HasValue ? _searchName2 : null, name, "String max has failed");
				var integ = await (dcQuery.MaxAsync(dc => dc.Integer, cancellationToken));
				Assert.AreEqual(expectedResult, integ, "Integer max has failed");
				var longInt = await (dcQuery.MaxAsync(dc => dc.Long, cancellationToken));
				Assert.AreEqual(expectedResult, longInt, "Long integer max has failed");
				var dec = await (dcQuery.MaxAsync(dc => dc.Decimal, cancellationToken));
				Assert.AreEqual(expectedResult, dec, "Decimal max has failed");
				var dbl = await (dcQuery.MaxAsync(dc => dc.Double, cancellationToken));
				Assert.AreEqual(expectedResult.HasValue, dbl.HasValue, "Double max has failed");
				if (expectedResult.HasValue)
					Assert.AreEqual(expectedResult.Value, dbl.Value, 0.001d, "Double max has failed");

				var date = await (dcQuery.MaxAsync(dc => dc.DateTime, cancellationToken));
				var dateWithOffset = await (dcQuery.MaxAsync(dc => dc.DateTimeOffset, cancellationToken));
				if (expectedResult.HasValue)
				{
					Assert.Greater(date, _testDate, "DateTime max has failed");
					Assert.Greater(dateWithOffset, _testDateWithOffset, "DateTimeOffset max has failed");
				}
				else
				{
					Assert.Null(date, "DateTime max has failed");
					Assert.Null(dateWithOffset, "DateTimeOffset max has failed");
				}

				if (expectedResult.HasValue)
				{
					var nonNullableDecimal = -1m;
					Assert.DoesNotThrowAsync(async () => { nonNullableDecimal = await (dcQuery.MaxAsync(dc => dc.NonNullableDecimal, cancellationToken)); }, "Non nullable decimal max has failed");
					Assert.AreEqual(expectedResult, nonNullableDecimal, "Non nullable decimal max has failed");
				}
				else
				{
					Assert.That(() => { dcQuery.Max(dc => dc.NonNullableDecimal); },
						// After fix
						Throws.InstanceOf<InvalidOperationException>()
						// Before fix
						.Or.InnerException.InstanceOf<ArgumentNullException>(),
						"Non nullable decimal max has failed");
				}
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task MinBBaseAsync()
		{
			return MinAsync<DomainClassBExtendedByA>(1);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task MinCBaseAsync()
		{
			return MinAsync<DomainClassCExtendedByD>(1);
		}

		// Non-reg case
		[Test]
		public Task MinEAsync()
		{
			return MinAsync<DomainClassE>(1);
		}

		// Non-reg case
		[Test]
		public Task MinFAsync()
		{
			return MinAsync<DomainClassF>(null);
		}

		// Non-reg case
		[Test]
		public Task MinGBaseAsync()
		{
			return MinAsync<DomainClassGExtendedByH>(1);
		}

		private async Task MinAsync<DC>(int? expectedResult, CancellationToken cancellationToken = default(CancellationToken)) where DC : DomainClassBase
		{
			using (var session = OpenSession())
			{
				var dcQuery = session.Query<DC>();
				var name = await (dcQuery.MinAsync(dc => dc.Name, cancellationToken));
				Assert.AreEqual(expectedResult.HasValue ? _searchName1 : null, name, "String min has failed");
				var integ = await (dcQuery.MinAsync(dc => dc.Integer, cancellationToken));
				Assert.AreEqual(expectedResult, integ, "Integer min has failed");
				var longInt = await (dcQuery.MinAsync(dc => dc.Long, cancellationToken));
				Assert.AreEqual(expectedResult, longInt, "Long integer min has failed");
				var dec = await (dcQuery.MinAsync(dc => dc.Decimal, cancellationToken));
				Assert.AreEqual(expectedResult, dec, "Decimal min has failed");
				var dbl = await (dcQuery.MinAsync(dc => dc.Double, cancellationToken));
				Assert.AreEqual(expectedResult.HasValue, dbl.HasValue, "Double min has failed");
				if (expectedResult.HasValue)
					Assert.AreEqual(expectedResult.Value, dbl.Value, 0.001d, "Double min has failed");

				var date = await (dcQuery.MinAsync(dc => dc.DateTime, cancellationToken));
				var dateWithOffset = await (dcQuery.MinAsync(dc => dc.DateTimeOffset, cancellationToken));
				if (expectedResult.HasValue)
				{
					Assert.Less(date, _testDate, "DateTime min has failed");
					Assert.Less(dateWithOffset, _testDateWithOffset, "DateTimeOffset min has failed");
				}
				else
				{
					Assert.Null(date, "DateTime min has failed");
					Assert.Null(dateWithOffset, "DateTimeOffset min has failed");
				}

				if (expectedResult.HasValue)
				{
					var nonNullableDecimal = -1m;
					Assert.DoesNotThrowAsync(async () => { nonNullableDecimal = await (dcQuery.MinAsync(dc => dc.NonNullableDecimal, cancellationToken)); }, "Non nullable decimal min has failed");
					Assert.AreEqual(expectedResult, nonNullableDecimal, "Non nullable decimal min has failed");
				}
				else
				{
					Assert.That(() => { dcQuery.Min(dc => dc.NonNullableDecimal); },
						// After fix
						Throws.InstanceOf<InvalidOperationException>()
						// Before fix
						.Or.InnerException.InstanceOf<ArgumentNullException>(),
						"Non nullable decimal min has failed");
				}
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultBBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassBExtendedByA>();
				DomainClassBExtendedByA result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync()); });
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultBBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassBExtendedByA>();
				DomainClassBExtendedByA result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.SingleOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassBExtendedByA>(result);
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultCBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassCExtendedByD>();
				DomainClassCExtendedByD result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync()); });
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultCBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassCExtendedByD>();
				DomainClassCExtendedByD result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.SingleOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassCExtendedByD>(result);
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultEAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassE>();
				DomainClassE result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync()); });
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultEWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassE>();
				DomainClassE result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.SingleOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNotNull(result);
				Assert.AreEqual(_searchName1, result.Name);
				Assert.IsInstanceOf<DomainClassE>(result);
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultFAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassF>();
				DomainClassF result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.SingleOrDefaultAsync()); });
				Assert.IsNull(result);
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultFWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassF>();
				DomainClassF result = null;
				Assert.DoesNotThrowAsync(async () => { result = await (query.SingleOrDefaultAsync(dc => dc.Name == _searchName1)); });
				Assert.IsNull(result);
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultGBaseAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassGExtendedByH>();
				DomainClassGExtendedByH result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync()); });
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultGBaseWithNameAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<DomainClassGExtendedByH>();
				DomainClassGExtendedByH result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync(dc => dc.Name == _searchName1)); });
			}
		}

		// Non-reg case
		[Test]
		public void SingleOrDefaultObjectAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<object>();
				object result = null;
				Assert.ThrowsAsync<InvalidOperationException>(async () => { result = await (query.SingleOrDefaultAsync()); });
			}
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task SumBBaseAsync()
		{
			return SumAsync<DomainClassBExtendedByA>(3);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task SumCBaseAsync()
		{
			return SumAsync<DomainClassCExtendedByD>(3);
		}

		// Non-reg case
		[Test]
		public Task SumEAsync()
		{
			return SumAsync<DomainClassE>(3);
		}

		// Non-reg case
		[Test]
		public Task SumFAsync()
		{
			return SumAsync<DomainClassF>(null);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public Task SumGBaseAsync()
		{
			return SumAsync<DomainClassGExtendedByH>(10);
		}

		// Failing case till NH-3850 is fixed
		[Test]
		public async Task SumObjectAsync()
		{
			using (var session = OpenSession())
			{
				var result = await (session.Query<object>().SumAsync(o => (int?)2));
				Assert.AreEqual(_totalEntityCount * 2, result);
			}
		}

		private async Task SumAsync<DC>(int? expectedResult, CancellationToken cancellationToken = default(CancellationToken)) where DC : DomainClassBase
		{
			using (var session = OpenSession())
			{
				var dcQuery = session.Query<DC>();
				var integ = await (dcQuery.SumAsync(dc => dc.Integer, cancellationToken));
				Assert.AreEqual(expectedResult, integ, "Integer sum has failed");
				var longInt = await (dcQuery.SumAsync(dc => dc.Long, cancellationToken));
				Assert.AreEqual(expectedResult, longInt, "Long integer sum has failed");
				var dec = await (dcQuery.SumAsync(dc => dc.Decimal, cancellationToken));
				Assert.AreEqual(expectedResult, dec, "Decimal sum has failed");
				var dbl = await (dcQuery.SumAsync(dc => dc.Double, cancellationToken));
				Assert.AreEqual(expectedResult.HasValue, dbl.HasValue, "Double sum has failed");
				if (expectedResult.HasValue)
					Assert.AreEqual(expectedResult.Value, dbl.Value, 0.001d, "Double sum has failed");

				if (expectedResult.HasValue)
				{
					var nonNullableDecimal = -1m;
					Assert.DoesNotThrowAsync(async () => { nonNullableDecimal = await (dcQuery.SumAsync(dc => dc.NonNullableDecimal, cancellationToken)); }, "Non nullable decimal sum has failed");
					Assert.AreEqual(expectedResult, nonNullableDecimal, "Non nullable decimal sum has failed");
				}
				else
				{
					Assert.That(() => { dcQuery.Sum(dc => dc.NonNullableDecimal); },
						// After fix
						Throws.InstanceOf<InvalidOperationException>()
						// Before fix
						.Or.InnerException.InstanceOf<ArgumentNullException>(),
						"Non nullable decimal sum has failed");
				}
			}
		}
	}
}