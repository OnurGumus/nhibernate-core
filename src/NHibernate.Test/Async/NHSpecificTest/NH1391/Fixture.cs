#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private object _idOfPersonWithAnimals;
		private object _idOfPersonWithCats;
		private object _idOfPersonWithDogs;
		private object _idOfPersonWithSivasKangals;
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					PersonWithAnimals personWithAnimals = new PersonWithAnimals{Name = "fabio"};
					PersonWithCats personWithCats = new PersonWithCats{Name = "dario"};
					PersonWithSivasKangals personWithSivasKangals = new PersonWithSivasKangals{Name = "tuna"};
					PersonWithDogs personWithDogs = new PersonWithDogs{Name = "davy"};
					var animalForAnimals = new Animal{Name = "Pasha", Owner = personWithAnimals};
					var dogForAnimals = new Dog{Name = "Efe", Country = "Turkey", Owner = personWithAnimals};
					var catForAnimals = new Cat{Name = "Tekir", EyeColor = "green", Owner = personWithAnimals};
					var sivasKangalForAnimals = new SivasKangal{Name = "Karabas", Country = "Turkey", HouseAddress = "Atakoy", Owner = personWithAnimals};
					personWithAnimals.AnimalsGeneric.Add(animalForAnimals);
					personWithAnimals.AnimalsGeneric.Add(dogForAnimals);
					personWithAnimals.AnimalsGeneric.Add(catForAnimals);
					personWithAnimals.AnimalsGeneric.Add(sivasKangalForAnimals);
					var animalForCats = new Animal{Name = "Pasha2", Owner = personWithCats};
					var catForCats = new Cat{Name = "Tekir2", EyeColor = "green", Owner = personWithCats};
					var dogForCats = new Dog{Name = "Efe2", Country = "Turkey", Owner = personWithCats};
					personWithCats.AnimalsGeneric.Add(catForCats);
					var catForDogs = new Cat{Name = "Tekir3", EyeColor = "blue", Owner = personWithDogs};
					var dogForDogs = new Dog{Name = "Efe3", Country = "Turkey", Owner = personWithDogs};
					var sivasKangalForDogs = new SivasKangal{Name = "Karabas3", Country = "Turkey", HouseAddress = "Atakoy", Owner = personWithDogs};
					personWithDogs.AnimalsGeneric.Add(dogForDogs);
					personWithDogs.AnimalsGeneric.Add(sivasKangalForDogs);
					var animalForSivasKangals = new Animal{Name = "Pasha4", Owner = personWithSivasKangals};
					var dogForSivasKangals = new Dog{Name = "Efe4", Country = "Turkey", Owner = personWithSivasKangals};
					var catForSivasKangals = new Cat{EyeColor = "red", Name = "Tekir4", Owner = personWithSivasKangals};
					var sivasKangalForSivasKangals = new SivasKangal{Name = "Karabas4", Country = "Turkey", HouseAddress = "Atakoy", Owner = personWithSivasKangals};
					personWithSivasKangals.AnimalsGeneric.Add(sivasKangalForSivasKangals);
					await (session.SaveAsync(animalForCats));
					await (session.SaveAsync(dogForCats));
					await (session.SaveAsync(catForDogs));
					await (session.SaveAsync(animalForSivasKangals));
					await (session.SaveAsync(dogForSivasKangals));
					await (session.SaveAsync(catForSivasKangals));
					_idOfPersonWithAnimals = await (session.SaveAsync(personWithAnimals));
					_idOfPersonWithCats = await (session.SaveAsync(personWithCats));
					_idOfPersonWithDogs = await (session.SaveAsync(personWithDogs));
					_idOfPersonWithSivasKangals = await (session.SaveAsync(personWithSivasKangals));
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
		public async Task Can_discriminate_subclass_on_list_with_lazy_loading_when_used_getAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					var personWithAnimals = await (session.GetAsync<PersonWithAnimals>(_idOfPersonWithAnimals));
					var personWithCats = await (session.GetAsync<PersonWithCats>(_idOfPersonWithCats));
					var personWithDogs = await (session.GetAsync<PersonWithDogs>(_idOfPersonWithDogs));
					var personWithSivasKangals = await (session.GetAsync<PersonWithSivasKangals>(_idOfPersonWithSivasKangals));
					Assert.That(personWithAnimals.AnimalsGeneric, Has.Count.EqualTo(4));
					Assert.That(personWithCats.CatsGeneric, Has.Count.EqualTo(1));
					Assert.That(personWithDogs.DogsGeneric, Has.Count.EqualTo(2));
					Assert.That(personWithSivasKangals.SivasKangalsGeneric, Has.Count.EqualTo(1));
				}
		}
	}
}
#endif
