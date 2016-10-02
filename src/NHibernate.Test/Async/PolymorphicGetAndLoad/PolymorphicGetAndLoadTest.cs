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
	public partial class PolymorphicGetAndLoadTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"PolymorphicGetAndLoad.Mappings.hbm.xml"};
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ScenarioWithA : IDisposable
		{
			private readonly ISessionFactory factory;
			private readonly A a;
			public ScenarioWithA(ISessionFactory factory)
			{
				this.factory = factory;
				a = new A{Name = "Patrick"};
				using (var s = factory.OpenSession())
				{
					s.Save(a);
					s.Flush();
				}
			}

			public A A
			{
				get
				{
					return a;
				}
			}

			public void Dispose()
			{
				using (var s = factory.OpenSession())
				{
					s.Delete(a);
					s.Flush();
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ScenarioWithB : IDisposable
		{
			private readonly ISessionFactory factory;
			private readonly B b;
			public ScenarioWithB(ISessionFactory factory)
			{
				this.factory = factory;
				b = new B{Name = "Patrick", Occupation = "hincha pelotas (en el buen sentido), but good candidate to be committer."};
				using (var s = factory.OpenSession())
				{
					s.Save(b);
					s.Flush();
				}
			}

			public B B
			{
				get
				{
					return b;
				}
			}

			public void Dispose()
			{
				using (var s = factory.OpenSession())
				{
					s.Delete(b);
					s.Flush();
				}
			}
		}

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
		public async Task WhenLoadBaseClassUsingInterfaceThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithA(Sfi))
			{
				using (var s = OpenSession())
				{
					Assert.That(async () => await (s.LoadAsync<INamed>(scenario.A.Id)), Throws.Nothing);
				}
			}
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
		public async Task WhenLoadInheritedClassUsingInterfaceThenNotThrowsAsync()
		{
			using (var scenario = new ScenarioWithB(Sfi))
			{
				using (var s = OpenSession())
				{
					Assert.That(async () => await (s.LoadAsync<INamed>(scenario.B.Id)), Throws.Nothing);
				}
			}
		}

		[Test]
		public async Task WhenLoadInheritedClassUsingInterfaceThenShouldAllowNarrowingProxyAsync()
		{
			using (var scenario = new ScenarioWithB(Sfi))
			{
				using (var s = OpenSession())
				{
					INamed loadedEntity = null;
					Assert.That(async () => loadedEntity = await (s.LoadAsync<INamed>(scenario.B.Id)), Throws.Nothing);
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(loadedEntity), Is.EqualTo(typeof (A)));
					var narrowedProxy = await (s.LoadAsync<B>(scenario.B.Id));
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(narrowedProxy), Is.EqualTo(typeof (B)));
					var firstLoadedImpl = ((INHibernateProxy)loadedEntity).HibernateLazyInitializer.GetImplementation((ISessionImplementor)s);
					var secondLoadedImpl = ((INHibernateProxy)narrowedProxy).HibernateLazyInitializer.GetImplementation((ISessionImplementor)s);
					Assert.That(firstLoadedImpl, Is.SameAs(secondLoadedImpl));
				}
			}
		}

		[Test]
		public async Task WhenLoadInterfaceThenShouldAllowNarrowingProxyAsync()
		{
			using (var scenario = new ScenarioWithB(Sfi))
			{
				using (var s = OpenSession())
				{
					INamed loadedEntity = null;
					Assert.That(async () => loadedEntity = await (s.LoadAsync<INamed>(scenario.B.Id)), Throws.Nothing);
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(loadedEntity), Is.EqualTo(typeof (A)));
					var narrowedProxy = await (s.LoadAsync<IOccuped>(scenario.B.Id));
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(narrowedProxy), Is.EqualTo(typeof (B)));
					var firstLoadedImpl = ((INHibernateProxy)loadedEntity).HibernateLazyInitializer.GetImplementation((ISessionImplementor)s);
					var secondLoadedImpl = ((INHibernateProxy)narrowedProxy).HibernateLazyInitializer.GetImplementation((ISessionImplementor)s);
					Assert.That(firstLoadedImpl, Is.SameAs(secondLoadedImpl));
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
		public async Task WhenLoadClassUsingInterfaceOfMultippleHierarchyThenThrowsAsync()
		{
			using (var s = OpenSession())
			{
				Assert.That(async () => await (s.LoadAsync<IMultiGraphNamed>(1)), Throws.TypeOf<HibernateException>().And.Message.ContainsSubstring("Ambiguous").And.Message.ContainsSubstring("GraphA").And.Message.ContainsSubstring("GraphB").And.Message.ContainsSubstring("IMultiGraphNamed"));
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
