using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Id.Insert
{
	/// <summary> 
	/// Responsible for handling delegation relating to variants in how
	/// insert-generated-identifier generator strategies dictate processing:
	/// <ul>
	/// <li>building the sql insert statement</li>
	/// <li>determination of the generated identifier value</li>
	/// </ul> 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IInsertGeneratedIdentifierDelegate
	{
		Task<
		/// <summary> 
		/// Perform the indicated insert SQL statement and determine the identifier value generated. 
		/// </summary>
		/// <param name = "insertSQL"> </param>
		/// <param name = "session"> </param>
		/// <param name = "binder"> </param>
		/// <returns> The generated identifier value. </returns>
		object> PerformInsertAsync(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder);
	}
}