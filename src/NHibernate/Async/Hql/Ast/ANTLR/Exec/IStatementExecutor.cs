#if NET_4_5
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IStatementExecutor
	{
		/// <summary> 
		/// Execute the sql managed by this executor using the given parameters. 
		/// </summary>
		/// <param name = "parameters">Essentially bind information for this processing. </param>
		/// <param name = "session">The session originating the request. </param>
		/// <returns> The number of entities updated/deleted. </returns>
		/// <exception cref = "HibernateException"/>
		Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session);
	}
}
#endif
