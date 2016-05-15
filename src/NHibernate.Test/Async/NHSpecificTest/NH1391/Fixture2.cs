#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture2 : BugTestCase
	{
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
