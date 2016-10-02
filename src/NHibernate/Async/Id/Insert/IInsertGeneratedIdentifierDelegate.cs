#if NET_4_5
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IInsertGeneratedIdentifierDelegate
	{
		/// <summary> 
		/// Perform the indicated insert SQL statement and determine the identifier value generated. 
		/// </summary>
		/// <param name = "insertSQL"> </param>
		/// <param name = "session"> </param>
		/// <param name = "binder"> </param>
		/// <returns> The generated identifier value. </returns>
		Task<object> PerformInsertAsync(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder);
	}
}
#endif
