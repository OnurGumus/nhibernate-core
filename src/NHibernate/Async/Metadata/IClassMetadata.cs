#if NET_4_5
using System.Collections;
using NHibernate.Type;
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Metadata
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IClassMetadata
	{
		/// <summary> Return the values of the mapped properties of the object</summary>
		Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session);
	}
}
#endif
