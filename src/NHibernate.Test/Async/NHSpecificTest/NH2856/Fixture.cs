﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2856
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : TestCaseMappingByCode
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.GenerateStatistics, "true");
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(rc =>
			{
				rc.Table("Person");
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Name);
				rc.ManyToOne(x => x.Address, m => m.Column("AddressId"));
			});

			mapper.Class<Address>(rc =>
			{
				rc.Table("Addresses");
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Name);
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public async Task EntityIsReturnedFromCacheOnSubsequentQueriesWhenUsingCacheableFetchQueryAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<Person>()
					.Fetch(p => p.Address)
					.Cacheable();

				sessions.Statistics.Clear();

				var result = await (query.ToListAsync()); // Execute the query

				Assert.That(result.Count, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(0));

				sessions.Statistics.Clear();

				var cachedResult = await (query.ToListAsync()); // Re-execute the query

				Assert.That(cachedResult.Count, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(0));
				Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(1));
			}
		}

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var person = new Person { Id = 5, Name = "Joe Bloggs" };
				session.Save(person);

				var address = new Address { Id = 15, Name = "Home" };
				session.Save(address);

				person.Address = address;
				session.Flush();

				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Delete("from Person");
				session.Delete("from Address");
				transaction.Commit();
			}
		}
	}
}