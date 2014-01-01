using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	/// <summary> 
	/// Encapsulates the strategy required to execute various types of update, delete,
	/// and insert statements issued through HQL. 
	/// </summary>
	public interface IStatementExecutor
	{
		SqlString[] SqlStatements { get; }

		/// <summary> 
		/// Execute the sql managed by this executor using the given parameters. 
		/// </summary>
		/// <param name="parameters">Essentially bind information for this processing. </param>
		/// <param name="session">The session originating the request. </param>
		/// <param name="async"></param>
		/// <returns> The number of entities updated/deleted. </returns>
		/// <exception cref="HibernateException"/>
		Task<int> Execute(QueryParameters parameters, ISessionImplementor session, bool async);
	}
}