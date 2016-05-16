using System.Xml;

namespace NHibernate.Test.TypesTest
{
	public partial class XmlDocClass
	{
		public int Id { get; set; }
		public XmlDocument Document { get; set; }
		public XmlDocument AutoDocument { get; set; }
	}
}