using System;

namespace NHibernate.SqlCommand
{
	public partial interface ISqlStringVisitor
	{
		void String(string text);
		void String(SqlString sqlString);
		void Parameter(Parameter parameter);
	}
}
