#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3634
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
		[Test]
		public async Task QueryAgainstComponentWithANullPropertyUsingCriteriaAsync()
		{
			//			Broken at the time NH3634 was reported
			//			Generates the following Rpc(exec sp_executesql)
			//			SELECT this_.Id as Id0_0_, 
			//				   this_.Name as Name0_0_, 
			//				   this_.ConnectionType as Connecti3_0_0_, 
			//				   this_.Address as Address0_0_, 
			//				   this_.PortName as PortName0_0_ 
			//			  FROM people this_ 
			//			 WHERE this_.ConnectionType = @p0 
			//			   and this_.Address = @p1 
			//			   and this_.PortName = @p2
			//
			//			@p0=N'http',@p1=N'test.com',@p2=NULL
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var componentToCompare = new Connection{ConnectionType = "http", Address = "test.com", PortName = null};
					var sally = await (session.CreateCriteria<Person>().Add(Restrictions.Eq("Connection", componentToCompare)).UniqueResultAsync<Person>());
					Assert.That(sally.Name, Is.EqualTo("Sally"));
					Assert.That(sally.Connection.PortName, Is.Null);
				}
		}

		[Test]
		public async Task CachedQueryMissesWithDifferentNotNullComponentAsync()
		{
			var componentToCompare = new Connection{ConnectionType = "http", Address = "test.com", PortName = null};
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var cached = await (session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).UniqueResultAsync<CachedPerson>());
					Assert.That(cached.Name, Is.EqualTo("CachedNull"));
					Assert.That(cached.Connection.PortName, Is.Null);
					using (var dbCommand = session.Connection.CreateCommand())
					{
						dbCommand.CommandText = "DELETE FROM cachedpeople";
						tx.Enlist(dbCommand);
						dbCommand.ExecuteNonQuery();
					}

					await (tx.CommitAsync());
				}

			componentToCompare.PortName = "port";
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//Cache should not return cached entity, because it no longer matches criteria
					var cachedPeople = session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).List<CachedPerson>();
					Assert.That(cachedPeople, Is.Empty);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CachedQueryMissesWithDifferentNullComponentAsync()
		{
			var componentToCompare = new Connection{ConnectionType = "http", Address = "test.com", PortName = "port"};
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var cached = await (session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).UniqueResultAsync<CachedPerson>());
					Assert.That(cached.Name, Is.EqualTo("CachedNotNull"));
					Assert.That(cached.Connection.PortName, Is.Not.Null);
					using (var dbCommand = session.Connection.CreateCommand())
					{
						dbCommand.CommandText = "DELETE FROM cachedpeople";
						tx.Enlist(dbCommand);
						dbCommand.ExecuteNonQuery();
					}

					await (tx.CommitAsync());
				}

			componentToCompare.PortName = null;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//Cache should not return cached entity, because it no longer matches criteria
					var cachedPeople = session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).List<CachedPerson>();
					Assert.That(cachedPeople, Is.Empty);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CachedQueryAgainstComponentWithANullPropertyUsingCriteriaAsync()
		{
			var componentToCompare = new Connection{ConnectionType = "http", Address = "test.com", PortName = null};
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var cached = await (session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).UniqueResultAsync<CachedPerson>());
					Assert.That(cached.Name, Is.EqualTo("CachedNull"));
					Assert.That(cached.Connection.PortName, Is.Null);
					using (var dbCommand = session.Connection.CreateCommand())
					{
						dbCommand.CommandText = "DELETE FROM cachedpeople";
						tx.Enlist(dbCommand);
						dbCommand.ExecuteNonQuery();
					}

					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//Should retreive from cache since we deleted directly from database.
					var cached = await (session.CreateCriteria<CachedPerson>().Add(Restrictions.Eq("Connection", componentToCompare)).SetCacheable(true).UniqueResultAsync<CachedPerson>());
					Assert.That(cached.Name, Is.EqualTo("CachedNull"));
					Assert.That(cached.Connection.PortName, Is.Null);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task QueryAgainstANullComponentPropertyUsingCriteriaApiAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var sally = await (session.CreateCriteria<Person>().Add(Restrictions.Eq("Connection.PortName", null)).Add(Restrictions.Eq("Connection.Address", "test.com")).Add(Restrictions.Eq("Connection.ConnectionType", "http")).UniqueResultAsync<Person>());
					Assert.That(sally.Name, Is.EqualTo("Sally"));
					Assert.That(sally.Connection.PortName, Is.Null);
				}
		}
	}
}
#endif
