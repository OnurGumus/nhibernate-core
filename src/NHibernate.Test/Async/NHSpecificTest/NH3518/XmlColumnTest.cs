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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XmlColumnTest : BugTestCase
	{
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
