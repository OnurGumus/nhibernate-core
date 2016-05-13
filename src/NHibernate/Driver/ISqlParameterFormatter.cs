using System;

namespace NHibernate.Driver
{
	public partial interface ISqlParameterFormatter
	{
		string GetParameterName(int index);
	}
}