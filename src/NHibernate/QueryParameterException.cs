using System;
using System.Runtime.Serialization;

namespace NHibernate
{
	[Serializable]
	public partial class QueryParameterException : QueryException
	{
		// TODO : without default constructor can't be serialized
		public QueryParameterException(string message) : base(message) { }
		public QueryParameterException(string message, Exception inner) : base(message, inner) { }
		protected QueryParameterException(SerializationInfo info,StreamingContext context): base(info, context) { }
	}
}
