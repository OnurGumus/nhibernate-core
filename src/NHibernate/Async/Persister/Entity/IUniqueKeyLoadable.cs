#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Persister.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IUniqueKeyLoadable
	{
		/// <summary>
		/// Load an instance of the persistent class, by a unique key other than the primary key.
		/// </summary>
		Task<object> LoadByUniqueKeyAsync(string propertyName, object uniqueKey, ISessionImplementor session);
	}
}
#endif
