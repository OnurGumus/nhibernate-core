using System.Reflection;
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Type
{
	/// <summary>
	/// Enables other Component-like types to hold collections and have cascades, etc.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IAbstractComponentType : IType
	{
		/// <summary>
		/// Get the values of the component properties of 
		/// a component instance
		/// </summary>
		Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session);
		/// <summary>
		/// Optional Operation
		/// </summary>
		Task<object[]> GetPropertyValuesAsync(object component, EntityMode entityMode);
		Task<object> GetPropertyValueAsync(object component, int i, ISessionImplementor session);
	}
}