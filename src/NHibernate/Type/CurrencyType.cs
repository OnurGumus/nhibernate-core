using System;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	[Serializable]
	public partial class CurrencyType : DecimalType
	{
		internal CurrencyType() : this(SqlTypeFactory.Currency) { }
		internal CurrencyType(SqlType sqlType) : base(sqlType) { }

		public override string Name
		{
			get { return "Currency"; }
		}
	}
}