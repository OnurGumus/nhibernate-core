#if NET_4_5
using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.PolymorphicGetAndLoad
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PolymorphicGetAndLoadTest : TestCase
	{
		[Test]
		public Task WhenSaveDeleteBaseClassCastedToInterfaceThenNotThrowsAsync()
		{
			try
			{
				WhenSaveDeleteBaseClassCastedToInterfaceThenNotThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenLoadBaseClassUsingInterfaceThenNotThrowsAsync()
		{
			try
			{
				WhenLoadBaseClassUsingInterfaceThenNotThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenGetBaseClassUsingInterfaceThenNotThrowsAsync()
		{
			try
			{
				WhenGetBaseClassUsingInterfaceThenNotThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenLoadInheritedClassUsingInterfaceThenNotThrowsAsync()
		{
			try
			{
				WhenLoadInheritedClassUsingInterfaceThenNotThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
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
					Assert.That(() => loadedEntity = s.Load<INamed>(scenario.B.Id), Throws.Nothing);
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(loadedEntity), Is.EqualTo(typeof (A)));
					var narrowedProxy = await (s.LoadAsync<B>(scenario.B.Id));
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(narrowedProxy), Is.EqualTo(typeof (B)));
					var firstLoadedImpl = await (((INHibernateProxy)loadedEntity).HibernateLazyInitializer.GetImplementationAsync((ISessionImplementor)s));
					var secondLoadedImpl = await (((INHibernateProxy)narrowedProxy).HibernateLazyInitializer.GetImplementationAsync((ISessionImplementor)s));
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
					Assert.That(() => loadedEntity = s.Load<INamed>(scenario.B.Id), Throws.Nothing);
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(loadedEntity), Is.EqualTo(typeof (A)));
					var narrowedProxy = await (s.LoadAsync<IOccuped>(scenario.B.Id));
					Assert.That(NHibernateProxyHelper.GetClassWithoutInitializingProxy(narrowedProxy), Is.EqualTo(typeof (B)));
					var firstLoadedImpl = await (((INHibernateProxy)loadedEntity).HibernateLazyInitializer.GetImplementationAsync((ISessionImplementor)s));
					var secondLoadedImpl = await (((INHibernateProxy)narrowedProxy).HibernateLazyInitializer.GetImplementationAsync((ISessionImplementor)s));
					Assert.That(firstLoadedImpl, Is.SameAs(secondLoadedImpl));
				}
			}
		}

		[Test]
		public Task WhenGetInheritedClassUsingInterfaceThenNotThrowsAsync()
		{
			try
			{
				WhenGetInheritedClassUsingInterfaceThenNotThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenLoadClassUsingInterfaceOfMultippleHierarchyThenThrowsAsync()
		{
			try
			{
				WhenLoadClassUsingInterfaceOfMultippleHierarchyThenThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenGetClassUsingInterfaceOfMultippleHierarchyThenThrowsAsync()
		{
			try
			{
				WhenGetClassUsingInterfaceOfMultippleHierarchyThenThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
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
					Assert.That(() => s.Get<INamed>(id), Throws.Nothing);
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
					Assert.That(() => s.Get<INamed>(id), Throws.Nothing);
				}
			}
		}
	}
}
#endif
