using System.Linq.Expressions;
using NHibernate.Hql.Ast;

namespace NHibernate.Linq.Visitors
{
	public partial interface IHqlExpressionVisitor
	{
		ISessionFactory SessionFactory { get; }

		HqlTreeNode Visit(Expression expression);
	}
}
