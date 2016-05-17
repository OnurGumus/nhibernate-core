#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using NHibernate.DomainModel;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.QueryTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CountFixture
	{
		[Test]
		public async Task DefaultAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.DomainModel.Simple.hbm.xml", typeof (Simple).Assembly);
			cfg.SetProperty(Environment.Hbm2ddlAuto, "create-drop");
			ISessionFactory sf = cfg.BuildSessionFactory();
			using (ISession s = sf.OpenSession())
			{
				object count = await (s.CreateQuery("select count(*) from Simple").UniqueResultAsync());
				Assert.IsTrue(count is Int64);
			}

			sf.Close();
		}

		[Test]
		public async Task OverriddenAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.SetProperty(Environment.Hbm2ddlAuto, "create-drop");
			cfg.AddResource("NHibernate.DomainModel.Simple.hbm.xml", typeof (Simple).Assembly);
			cfg.AddSqlFunction("count", new ClassicCountFunction());
			ISessionFactory sf = cfg.BuildSessionFactory();
			using (ISession s = sf.OpenSession())
			{
				object count = await (s.CreateQuery("select count(*) from Simple").UniqueResultAsync());
				Assert.IsTrue(count is Int32);
			}

			sf.Close();
		}
	}
}
#endif
