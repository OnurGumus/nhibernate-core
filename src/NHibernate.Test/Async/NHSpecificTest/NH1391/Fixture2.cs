#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture2Async : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					PersonWithAllTypes personWithAllTypes = new PersonWithAllTypes();
					Animal animal = new Animal{Name = "Pasha", Owner = personWithAllTypes};
					Dog dog = new Dog{Country = "Turkey", Name = "Kral", Owner = personWithAllTypes};
					SivasKangal sivasKangal = new SivasKangal{Name = "Karabas", Country = "Turkey", HouseAddress = "Address", Owner = personWithAllTypes};
					Cat cat = new Cat{Name = "Tekir", EyeColor = "Red", Owner = personWithAllTypes};
					personWithAllTypes.AnimalsGeneric.Add(animal);
					personWithAllTypes.AnimalsGeneric.Add(cat);
					personWithAllTypes.AnimalsGeneric.Add(dog);
					personWithAllTypes.AnimalsGeneric.Add(sivasKangal);
					await (session.SaveAsync(personWithAllTypes));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Animal"));
					await (session.DeleteAsync("from Person"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task Can_discriminate_subclass_on_list_with_lazy_loading_when_used_and_person_had_multiple_listAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					var personWithAnimals = await (session.GetAsync<PersonWithAllTypes>(1));
					Assert.That(personWithAnimals.AnimalsGeneric, Has.Count.EqualTo(4));
					Assert.That(personWithAnimals.CatsGeneric, Has.Count.EqualTo(1));
					Assert.That(personWithAnimals.DogsGeneric, Has.Count.EqualTo(2));
					Assert.That(personWithAnimals.SivasKangalsGeneric, Has.Count.EqualTo(1));
				}
		}
	}
}
#endif
