using System.Threading.Tasks;
using NHibernate.Engine;

namespace NHibernate.Loader.Collection
{
	/// <summary>
	/// An interface for collection loaders
	/// </summary>
	/// <seealso cref="BasicCollectionLoader"/>
	/// <seealso cref="OneToManyLoader"/>
	public interface ICollectionInitializer
	{
		/// <summary>
		/// Initialize the given collection
		/// </summary>
		Task Initialize(object id, ISessionImplementor session);
	}
}