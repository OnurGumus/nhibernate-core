#if NET_4_5
using System.IO;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CfgTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MappingDocumentParserTestsAsync
	{
		[Test]
		public void CanDeserializeHBM()
		{
			string[] someEmbeddedResources = {"NHibernate.DomainModel.ABC.hbm.xml", "NHibernate.DomainModel.ABCProxy.hbm.xml", };
			Assembly domainModelAssembly = typeof (DomainModel.A).Assembly;
			MappingDocumentParser parser = new MappingDocumentParser();
			foreach (string embeddedResource in someEmbeddedResources)
				using (Stream stream = domainModelAssembly.GetManifestResourceStream(embeddedResource))
				{
					HbmMapping mapping = parser.Parse(stream);
					Assert.IsNotNull(mapping, "Mapping: " + embeddedResource);
				}
		}
	}
}
#endif
