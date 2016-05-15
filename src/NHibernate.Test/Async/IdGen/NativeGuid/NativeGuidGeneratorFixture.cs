#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Id;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.NativeGuid
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeGuidGeneratorFixture
	{
		[Test]
		public async Task ReturnedValueIsGuidAsync()
		{
			try
			{
				var str = Dialect.Dialect.GetDialect().SelectGUIDString;
			}
			catch (NotSupportedException)
			{
				Assert.Ignore("This test does not apply to {0}", Dialect.Dialect.GetDialect());
			}

			var gen = new NativeGuidGenerator();
			using (ISession s = sessions.OpenSession())
			{
				object result = await (gen.GenerateAsync((ISessionImplementor)s, null));
				Assert.That(result, Is.TypeOf(typeof (Guid)));
				Assert.That(result, Is.Not.EqualTo(Guid.Empty));
			}
		}
	}
}
#endif
