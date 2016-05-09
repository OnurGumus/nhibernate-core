using System;

namespace NHibernate.Hql.Ast.ANTLR
{
	public partial class DetailedSemanticException : SemanticException
	{
		public DetailedSemanticException(string message) : base(message)
		{
		}

		public DetailedSemanticException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
