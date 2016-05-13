using System;

namespace NHibernate.Mapping
{
	public partial interface ITableOwner
	{
		Table Table { set; }
	}
}