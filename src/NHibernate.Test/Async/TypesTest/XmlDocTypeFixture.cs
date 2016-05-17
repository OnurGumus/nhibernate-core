#if NET_4_5
using System.Data;
using System.Xml;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XmlDocTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			using (var s = OpenSession())
			{
				var docEntity = new XmlDocClass{Id = 1};
				docEntity.Document = new XmlDocument();
				docEntity.Document.LoadXml("<MyNode>my Text</MyNode>");
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XmlDocClass>(1));
				var document = docEntity.Document;
				Assert.That(document, Is.Not.Null);
				Assert.That(document.OuterXml, Is.StringContaining("<MyNode>my Text</MyNode>"));
				var xmlElement = document.CreateElement("Pizza");
				xmlElement.SetAttribute("temp", "calda");
				document.FirstChild.AppendChild(xmlElement);
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XmlDocClass>(1));
				Assert.That(docEntity.Document.OuterXml, Is.StringContaining("Pizza temp=\"calda\""));
				await (s.DeleteAsync(docEntity));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task InsertNullValueAsync()
		{
			using (ISession s = OpenSession())
			{
				var docEntity = new XmlDocClass{Id = 1};
				docEntity.Document = null;
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XmlDocClass>(1));
				Assert.That(docEntity.Document, Is.Null);
				await (s.DeleteAsync(docEntity));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
