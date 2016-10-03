#if NET_4_5
using System.Collections;
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Tuple.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEntityTuplizer : ITuplizer
	{
		/// <summary> Extract the values of the insertable properties of the entity (including backrefs) </summary>
		/// <param name = "entity">The entity from which to extract. </param>
		/// <param name = "mergeMap">a map of instances being merged to merged instances </param>
		/// <param name = "session">The session in which the request is being made. </param>
		/// <returns> The insertable property values. </returns>
		Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session);
	}
}
#endif
