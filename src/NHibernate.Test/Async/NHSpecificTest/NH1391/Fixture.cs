#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
