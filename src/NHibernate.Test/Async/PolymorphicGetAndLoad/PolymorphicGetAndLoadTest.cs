#if NET_4_5
using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PolymorphicGetAndLoad
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PolymorphicGetAndLoadTest : TestCase
	{
		[Test]
		public async Task WhenSaveDeleteBaseClassCastedToInterfaceThenNotThrowsAsync()
		{
			INamed a = new A{Name = "Patrick"};
			Assert.That(async () =>
			{
				using (var s = OpenSession())
				{
					await (s.SaveAsync(a));
					await (s.FlushAsync());
				}
			}

			, Throws.Nothing);
			Assert.That(async () =>
			{
				using (var s = OpenSession())
				{
					await (s.DeleteAsync(a));
					await (s.FlushAsync());
				}
			}

			, Throws.Nothing);
		}

		[Test]
		public async Task WhenGetBaseClassUsingInterfaceThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithA(Sfi))
			{
				using (var s = OpenSession())
				{
					Assert.That(async () => await (s.GetAsync<INamed>(scenario.A.Id)), Throws.Nothing);
				}
			}
		}

		[Test]
		public async Task WhenGetInheritedClassUsingInterfaceThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithB(Sfi))
			{
				using (var s = OpenSession())
				{
					INamed loadedEntity = null;
					Assert.That(async () => loadedEntity = await (s.GetAsync<INamed>(scenario.B.Id)), Throws.Nothing);
					Assert.That(loadedEntity, Is.TypeOf<B>());
				}
			}
		}

		[Test]
		public async Task WhenGetClassUsingInterfaceOfMultippleHierarchyThenThrowsAsync()
		{
			using (var s = OpenSession())
			{
				Assert.That(async () => await (s.GetAsync<IMultiGraphNamed>(1)), Throws.TypeOf<HibernateException>().And.Message.StringContaining("Ambiguous").And.Message.StringContaining("GraphA").And.Message.StringContaining("GraphB").And.Message.StringContaining("IMultiGraphNamed"));
			}
		}

		[Test]
		public async Task WhenGetBaseClassUsingInterfaceFromSessionCacheThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithA(Sfi))
			{
				using (var s = OpenSession())
				{
					var id = scenario.A.Id;
					await (s.GetAsync<A>(id));
					Assert.That(async () => await (s.GetAsync<INamed>(id)), Throws.Nothing);
				}
			}
		}

		[Test]
		public async Task WhenGetInheritedClassUsingInterfaceFromSessionCacheThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithB(Sfi))
			{
				using (var s = OpenSession())
				{
					var id = scenario.B.Id;
					await (s.GetAsync<B>(id));
					Assert.That(async () => await (s.GetAsync<INamed>(id)), Throws.Nothing);
				}
			}
		}
	}
}
#endif
