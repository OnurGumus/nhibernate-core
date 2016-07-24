#if NET_4_5
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using NHibernate.Proxy.DynamicProxy;
using System.Threading.Tasks;

namespace NHibernate.Test.DynamicProxyTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PeVerifyFixture
	{
		[Test]
		public async Task VerifyClassWithPublicConstructorAsync()
		{
			var factory = new ProxyFactory(new SavingProxyAssemblyBuilder(assemblyName));
			var proxyType = factory.CreateProxyType(typeof (ClassWithPublicDefaultConstructor), null);
			wasCalled = false;
			Activator.CreateInstance(proxyType);
			Assert.That(wasCalled);
			await (new PeVerifier(assemblyFileName).AssertIsValidAsync());
		}

		[Test]
		public async Task VerifyClassWithProtectedConstructorAsync()
		{
			var factory = new ProxyFactory(new SavingProxyAssemblyBuilder(assemblyName));
			var proxyType = factory.CreateProxyType(typeof (ClassWithProtectedDefaultConstructor), null);
			wasCalled = false;
			Activator.CreateInstance(proxyType);
			Assert.That(wasCalled);
			await (new PeVerifier(assemblyFileName).AssertIsValidAsync());
		}
	}
}
#endif
