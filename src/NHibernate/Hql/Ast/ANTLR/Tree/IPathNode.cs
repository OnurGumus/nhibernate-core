namespace NHibernate.Hql.Ast.ANTLR.Tree
{
	/// <summary>
	/// An AST node with a path property.  This path property will be the fully qualified name.
	/// Author: josh
	/// Ported by: Steve Strong
	/// </summary>
	public partial interface IPathNode
	{
		/// <summary>
		/// Returns the full path name represented by the node.
		/// </summary>
		/// <returns>the full path name represented by the node.</returns>
		string Path { get; }
	}
}
