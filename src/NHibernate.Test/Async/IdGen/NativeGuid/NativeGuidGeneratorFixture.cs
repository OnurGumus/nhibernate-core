#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Id;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.NativeGuid
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeGuidGeneratorFixtureAsync
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
		}

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
