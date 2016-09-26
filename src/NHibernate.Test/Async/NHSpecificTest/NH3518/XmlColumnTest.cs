#if NET_4_5
using System.Xml;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3518
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XmlColumnTestAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenSession())
				using (var t = session.BeginTransaction())
				{
					await (session.DeleteAsync("from ClassWithXmlMember"));
					await (t.CommitAsync());
				}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override bool AppliesTo(ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver is SqlClientDriver;
		}

		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.PrepareSql, "true");
		}

		[Test]
		public async Task FilteredQueryAsync()
		{
			var xmlDocument = new XmlDocument();
			var xmlElement = xmlDocument.CreateElement("testXml");
			xmlDocument.AppendChild(xmlElement);
			var parentA = new ClassWithXmlMember("A", xmlDocument);
			using (var s = sessions.OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.SaveAsync(parentA));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
