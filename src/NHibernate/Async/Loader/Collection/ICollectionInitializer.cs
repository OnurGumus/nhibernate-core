#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Loader.Collection
{
	/// <summary>
	/// An interface for collection loaders
	/// </summary>
	/// <seealso cref = "BasicCollectionLoader"/>
	/// <seealso cref = "OneToManyLoader"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ICollectionInitializer
	{
		/// <summary>
		/// Initialize the given collection
		/// </summary>
		Task InitializeAsync(object id, ISessionImplementor session);
	}
}
#endif
