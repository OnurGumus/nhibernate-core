using System;

namespace NHibernate.Hql.Ast.ANTLR.Tree
{
	[CLSCompliant(false)]
	public partial interface IASTFactory
	{
		IASTNode CreateNode(int type, string text, params IASTNode[] children);
	}
}
