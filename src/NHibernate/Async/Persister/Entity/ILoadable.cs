using NHibernate.Type;
using NHibernate.Engine;
using System.Data;
using System.Threading.Tasks;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Implemented by <c>ClassPersister</c> that uses <c>Loader</c>. There are several optional
	/// operations used only by loaders that inherit <c>OuterJoinLoader</c>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILoadable : IEntityPersister
	{
		Task<
		/// <summary>
		/// Retrieve property values from one row of a result set
		/// </summary>
		object[]> HydrateAsync(IDataReader rs, object id, object obj, ILoadable rootLoadable, string[][] suffixedPropertyColumns, bool allProperties, ISessionImplementor session);
	}
}