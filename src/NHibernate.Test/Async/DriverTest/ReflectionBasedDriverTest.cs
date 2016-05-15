#if NET_4_5
using System;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DriverTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReflectionBasedDriverTest
	{
		[Test]
		public async Task WhenCreatedWithDbFactoryThenCanReturnConnectionAsync()
		{
			var provider = new MyDriverWithWrongClassesAndGoodDbProviderFactory();
			using (var connection = await (provider.CreateConnectionAsync()))
			{
				Assert.That(connection, Is.Not.Null);
			}
		}

		[Test]
		public async Task WhenCreatedWithNoDbFactoryThenCanReturnConnectionAsync()
		{
			var provider = new MyDriverWithNoDbProviderFactory();
			using (var connection = await (provider.CreateConnectionAsync()))
			{
				Assert.That(connection, Is.Not.Null);
			}
		}
	}
}
#endif
