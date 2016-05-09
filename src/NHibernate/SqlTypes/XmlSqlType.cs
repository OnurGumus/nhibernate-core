using System;
using System.Data;

namespace NHibernate.SqlTypes
{
	[Serializable]
	public partial class XmlSqlType : SqlType
	{
		public XmlSqlType()
			: base(DbType.Xml)
		{
		}

		public XmlSqlType(int length) : base(DbType.Xml, length)
		{
		}
	}
}