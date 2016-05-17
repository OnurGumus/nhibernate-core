#if NET_4_5
using System.Data;
using System.Xml.Linq;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XDocTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			using (var s = OpenSession())
			{
				var docEntity = new XDocClass{Id = 1};
				docEntity.Document = XDocument.Parse("<MyNode>my Text</MyNode>");
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XDocClass>(1));
				var document = docEntity.Document;
				Assert.That(document, Is.Not.Null);
				Assert.That(document.Document.Root.ToString(SaveOptions.DisableFormatting), Is.StringContaining("<MyNode>my Text</MyNode>"));
				var xmlElement = new XElement("Pizza", new XAttribute("temp", "calda"));
				document.Document.Root.Add(xmlElement);
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XDocClass>(1));
				var document = docEntity.Document;
				Assert.That(document.Document.Root.ToString(SaveOptions.DisableFormatting), Is.StringContaining("Pizza temp=\"calda\""));
				await (s.DeleteAsync(docEntity));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task InsertNullValueAsync()
		{
			using (ISession s = OpenSession())
			{
				var docEntity = new XDocClass{Id = 1};
				docEntity.Document = null;
				await (s.SaveAsync(docEntity));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				var docEntity = await (s.GetAsync<XDocClass>(1));
				Assert.That(docEntity.Document, Is.Null);
				await (s.DeleteAsync(docEntity));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
