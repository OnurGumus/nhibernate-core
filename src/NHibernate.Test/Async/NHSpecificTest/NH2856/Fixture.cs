#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2856
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.SetProperty(Environment.GenerateStatistics, "true");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
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
			}

			);
			mapper.Class<Address>(rc =>
			{
				rc.Table("Addresses");
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Name);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public void EntityIsReturnedFromCacheOnSubsequentQueriesWhenUsingCacheableFetchQuery()
		{
			using (var session = OpenSession())
			{
				var query = session.Query<Person>().Fetch(p => p.Address).Cacheable();
				sessions.Statistics.Clear();
				var result = query.ToList(); // Execute the query
				Assert.That(result.Count, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(0));
				sessions.Statistics.Clear();
				var cachedResult = query.ToList(); // Re-execute the query
				Assert.That(cachedResult.Count, Is.EqualTo(1));
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(0));
				Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(1));
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var person = new Person{Id = 5, Name = "Joe Bloggs"};
					await (session.SaveAsync(person));
					var address = new Address{Id = 15, Name = "Home"};
					await (session.SaveAsync(address));
					person.Address = address;
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (session.DeleteAsync("from Address"));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
