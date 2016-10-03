#if NET_4_5
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH845
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public async Task HbmOrdererForgetsMappingFilesWithoutClassesIfExtendsIsUsedAsync()
		{
			Configuration cfg = new Configuration();
			Assembly domain = typeof (Master).Assembly;
			cfg.AddResource("NHibernate.DomainModel.MasterDetail.hbm.xml", domain);
			cfg.AddResource("NHibernate.DomainModel.MultiExtends.hbm.xml", domain);
			cfg.AddResource("NHibernate.DomainModel.Multi.hbm.xml", domain);
			cfg.AddResource("NHibernate.DomainModel.Query.hbm.xml", domain);
			ISessionFactory sf = cfg.BuildSessionFactory();
			try
			{
				using (ISession session = sf.OpenSession())
				{
					Assert.IsNotNull(session.GetNamedQuery("AQuery"));
				}
			}
			finally
			{
				await (sf.CloseAsync());
			}
		}
	}
}
#endif
