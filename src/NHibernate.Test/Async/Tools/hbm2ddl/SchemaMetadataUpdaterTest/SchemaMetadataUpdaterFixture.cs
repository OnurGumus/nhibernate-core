#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Tools.hbm2ddl.SchemaMetadataUpdaterTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SchemaMetadataUpdaterFixture
	{
		[Test]
		public async Task CanWorkWithAutoQuoteTableAndColumnsAtStratupAsync()
		{
			var configuration = TestConfigurationHelper.GetDefaultConfiguration();
			configuration.SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote");
			configuration.SetProperty(Environment.Hbm2ddlAuto, "create-drop");
			configuration.AddResource("NHibernate.Test.Tools.hbm2ddl.SchemaMetadataUpdaterTest.HeavyEntity.hbm.xml", GetType().Assembly);
			var sf = configuration.BuildSessionFactory();
			using (ISession s = sf.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Order{From = "from", Column = "column", And = "order"}));
					await (t.CommitAsync());
				}

			using (ISession s = sf.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Order"));
					await (t.CommitAsync());
				}

			new SchemaExport(configuration).Drop(false, false);
		}
	}
}
#endif
