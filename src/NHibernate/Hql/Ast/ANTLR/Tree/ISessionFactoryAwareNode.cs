using NHibernate.Engine;

namespace NHibernate.Hql.Ast.ANTLR.Tree
{
	/// <summary>
	/// Interface for nodes which require access to the SessionFactory
	/// 
	/// Author: Steve Ebersole
	/// Ported by: Steve Strong
	/// </summary>
	public partial interface ISessionFactoryAwareNode
	{
		ISessionFactoryImplementor SessionFactory
		{ 
			set;
		}
	}
}
