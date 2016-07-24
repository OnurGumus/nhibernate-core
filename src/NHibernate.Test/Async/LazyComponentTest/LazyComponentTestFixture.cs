#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.LazyComponentTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyComponentTestFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"LazyComponentTest.Person.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var person = new Person{Name = "Gabor", Address = new Address{Country = "HUN", City = "Budapest"}};
					await (s.PersistAsync(person));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Person").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyLoadTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = await (s.CreateQuery("from Person p where name='Gabor'").UniqueResultAsync<Person>());
					// make sure component has not been initialized yet
					Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(p, "Address")), Is.False);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyDeleteTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = await (s.CreateQuery("from Person p where name='Gabor'").UniqueResultAsync<Person>());
					// make sure component has not been initialized yet
					Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(p, "Address")), Is.False);
					await (s.DeleteAsync(p));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyUpdateTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = await (s.CreateQuery("from Person p where name='Gabor'").UniqueResultAsync<Person>());
					// make sure component has not been initialized yet
					Assert.That(!await (NHibernateUtil.IsPropertyInitializedAsync(p, "Address")));
					p.Address.City = "Baja";
					await (s.UpdateAsync(p));
					await (t.CommitAsync());
				}

			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = await (s.CreateQuery("from Person p where name='Gabor'").UniqueResultAsync<Person>());
					Assert.That(p.Address.City, Is.EqualTo("Baja"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
