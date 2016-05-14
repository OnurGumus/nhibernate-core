#if NET_4_5
using NHibernate.Type;
using NHibernate.Engine;
using System.Data.Common;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Implemented by <c>ClassPersister</c> that uses <c>Loader</c>. There are several optional
	/// operations used only by loaders that inherit <c>OuterJoinLoader</c>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILoadable : IEntityPersister
	{
		/// <summary>
		/// Retrieve property values from one row of a result set
		/// </summary>
		Task<object[]> HydrateAsync(DbDataReader rs, object id, object obj, ILoadable rootLoadable, string[][] suffixedPropertyColumns, bool allProperties, ISessionImplementor session);
	}
}
#endif
