using System;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace NHibernate.Hql.Ast.ANTLR.Util
{
	[CLSCompliant(false)]
	public partial interface IVisitationStrategy
	{
		void Visit(IASTNode node);
	}
}