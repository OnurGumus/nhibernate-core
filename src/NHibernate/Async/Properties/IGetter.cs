using System.Collections;
using System.Reflection;
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Properties
{
	/// <summary>
	/// Gets values of a particular mapped property.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IGetter
	{
		/// <summary> Get the property value from the given owner instance. </summary>
		/// <param name = "owner">The instance containing the value to be retrieved. </param>
		/// <param name = "mergeMap">a map of merged persistent instances to detached instances </param>
		/// <param name = "session">The session from which this request originated. </param>
		/// <returns> The extracted value. </returns>
		Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session);
	}
}