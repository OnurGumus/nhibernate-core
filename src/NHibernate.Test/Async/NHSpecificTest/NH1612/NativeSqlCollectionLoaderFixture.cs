#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1612
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSqlCollectionLoaderFixture : BugTestCase
	{
		[Test]
		public async Task LoadElementsWithWithSimpleHbmAliasInjectionAsync()
		{
			string[] routes = CreateRoutes();
			Country country = await (LoadCountryWithNativeSQLAsync(CreateCountry(routes), "LoadCountryRoutesWithSimpleHbmAliasInjection"));
			Assert.That(country, Is.Not.Null);
			Assert.That(country.Routes, Is.EquivalentTo(routes));
			await (CleanupAsync());
		}

		[Test]
		public async Task LoadElementsWithExplicitColumnMappingsAsync()
		{
			string[] routes = CreateRoutes();
			Country country = await (LoadCountryWithNativeSQLAsync(CreateCountry(routes), "LoadCountryRoutesWithCustomAliases"));
			Assert.That(country, Is.Not.Null);
			Assert.That(country.Routes, Is.EquivalentTo(routes));
			await (CleanupAsync());
		}

		[Test]
		public async Task LoadCompositeElementsWithWithSimpleHbmAliasInjectionAsync()
		{
			IDictionary<int, AreaStatistics> stats = CreateStatistics();
			Country country = await (LoadCountryWithNativeSQLAsync(CreateCountry(stats), "LoadAreaStatisticsWithSimpleHbmAliasInjection"));
			Assert.That(country, Is.Not.Null);
			Assert.That((ICollection)country.Statistics.Keys, Is.EquivalentTo((ICollection)stats.Keys), "Keys");
			Assert.That((ICollection)country.Statistics.Values, Is.EquivalentTo((ICollection)stats.Values), "Elements");
			await (CleanupWithPersonsAsync());
		}

		[Test]
		public async Task LoadCompositeElementsWithWithComplexHbmAliasInjectionAsync()
		{
			IDictionary<int, AreaStatistics> stats = CreateStatistics();
			Country country = await (LoadCountryWithNativeSQLAsync(CreateCountry(stats), "LoadAreaStatisticsWithComplexHbmAliasInjection"));
			Assert.That(country, Is.Not.Null);
			Assert.That((ICollection)country.Statistics.Keys, Is.EquivalentTo((ICollection)stats.Keys), "Keys");
			Assert.That((ICollection)country.Statistics.Values, Is.EquivalentTo((ICollection)stats.Values), "Elements");
			await (CleanupWithPersonsAsync());
		}

		[Test]
		public async Task LoadCompositeElementsWithWithCustomAliasesAsync()
		{
			IDictionary<int, AreaStatistics> stats = CreateStatistics();
			Country country = await (LoadCountryWithNativeSQLAsync(CreateCountry(stats), "LoadAreaStatisticsWithCustomAliases"));
			Assert.That(country, Is.Not.Null);
			Assert.That((ICollection)country.Statistics.Keys, Is.EquivalentTo((ICollection)stats.Keys), "Keys");
			Assert.That((ICollection)country.Statistics.Values, Is.EquivalentTo((ICollection)stats.Values), "Elements");
			await (CleanupWithPersonsAsync());
		}

		[Test]
		public async Task LoadEntitiesWithWithSimpleHbmAliasInjectionAsync()
		{
			City[] cities = CreateCities();
			Country country = CreateCountry(cities);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var c = session.GetNamedQuery("LoadCountryCitiesWithSimpleHbmAliasInjection").SetString("country_code", country.Code).UniqueResult<Country>();
				Assert.That(c, Is.Not.Null);
				Assert.That(c.Cities, Is.EquivalentTo(cities));
			}

			await (CleanupWithCitiesAsync());
		}

		[Test]
		public async Task LoadEntitiesWithComplexHbmAliasInjectionAsync()
		{
			City[] cities = CreateCities();
			Country country = CreateCountry(cities);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var c = session.GetNamedQuery("LoadCountryCitiesWithComplexHbmAliasInjection").SetString("country_code", country.Code).UniqueResult<Country>();
				Assert.That(c, Is.Not.Null);
				Assert.That(c.Cities, Is.EquivalentTo(cities));
			}

			await (CleanupWithCitiesAsync());
		}

		[Test]
		public async Task LoadEntitiesWithExplicitColumnMappingsAsync()
		{
			City[] cities = CreateCities();
			Country country = CreateCountry(cities);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var c = session.GetNamedQuery("LoadCountryCitiesWithCustomAliases").SetString("country_code", country.Code).UniqueResult<Country>();
				Assert.That(c, Is.Not.Null);
				Assert.That(c.Cities, Is.EquivalentTo(cities));
			}

			await (CleanupWithCitiesAsync());
		}

		[Test]
		public async Task NativeQueryWithUnresolvedHbmAliasInjectionAsync()
		{
			IDictionary<int, AreaStatistics> stats = CreateStatistics();
			try
			{
				await (LoadCountryWithNativeSQLAsync(CreateCountry(stats), "LoadAreaStatisticsWithFaultyHbmAliasInjection"));
				Assert.Fail("Expected exception");
			}
			catch (QueryException)
			{
			// ok
			}
			finally
			{
				await (CleanupWithPersonsAsync());
			}
		}

		private async Task<Country> LoadCountryWithNativeSQLAsync(Country country, string queryName)
		{
			// Ensure country is saved and session cache is empty to force from now on the reload of all 
			// persistence objects from the database.
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(country));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				return session.GetNamedQuery(queryName).SetString("country_code", country.Code).UniqueResult<Country>();
			}
		}

		[Test]
		public async Task LoadElementCollectionWithCustomLoaderAsync()
		{
			string[] routes = CreateRoutes();
			Country country = CreateCountry(routes);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var c = await (session.GetAsync<Country>(country.Code));
				Assert.That(c, Is.Not.Null, "country");
				Assert.That(c.Routes, Is.EquivalentTo(routes), "country.Routes");
			}

			await (CleanupAsync());
		}

		[Test]
		public async Task LoadCompositeElementCollectionWithCustomLoaderAsync()
		{
			IDictionary<int, AreaStatistics> stats = CreateStatistics();
			Country country = CreateCountry(stats);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var a = await (session.GetAsync<Area>(country.Code));
				Assert.That(a, Is.Not.Null, "area");
				Assert.That((ICollection)a.Statistics.Keys, Is.EquivalentTo((ICollection)stats.Keys), "area.Keys");
				Assert.That((ICollection)a.Statistics.Values, Is.EquivalentTo((ICollection)stats.Values), "area.Elements");
			}

			await (CleanupWithPersonsAsync());
		}

		[Test]
		public async Task LoadEntityCollectionWithCustomLoaderAsync()
		{
			City[] cities = CreateCities();
			Country country = CreateCountry(cities);
			await (SaveAsync(country));
			using (ISession session = OpenSession())
			{
				var c = await (session.GetAsync<Country>(country.Code));
				Assert.That(c, Is.Not.Null, "country");
				Assert.That(c.Cities, Is.EquivalentTo(cities), "country.Cities");
			}

			await (CleanupWithCitiesAsync());
		}

		private async Task SaveAsync<TArea>(TArea area)where TArea : Area
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(area));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task NativeUpdateQueryWithoutResultsAsync()
		{
			if (!(Dialect is MsSql2000Dialect))
			{
				Assert.Ignore("This does not apply to {0}", Dialect);
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					session.GetNamedQuery("UpdateQueryWithoutResults").ExecuteUpdate();
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task NativeScalarQueryWithoutResultsAsync()
		{
			if (!(Dialect is MsSql2000Dialect))
			{
				Assert.Ignore("This does not apply to {0}", Dialect);
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					// Native SQL Query outcome is not validated against <return-*> 
					// resultset declarations.
					session.GetNamedQuery("ScalarQueryWithDefinedResultsetButNoResults").ExecuteUpdate();
					await (tx.CommitAsync());
				}
			}
		}

		private async Task CleanupAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task CleanupWithPersonsAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task CleanupWithCitiesAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from City"));
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
