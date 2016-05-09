using System;

namespace NHibernate.Hql
{
	public partial class QueryExecutionRequestException : QueryException
	{
		public QueryExecutionRequestException(string message, string queryString) : base(message, queryString)
		{
		}
	}
}