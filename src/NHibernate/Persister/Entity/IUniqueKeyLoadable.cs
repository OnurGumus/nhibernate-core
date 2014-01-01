using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Describes a class that may be loaded via a unique key.
	/// </summary>
	public interface IUniqueKeyLoadable
	{
		/// <summary>
		/// Load an instance of the persistent class, by a unique key other than the primary key.
		/// </summary>
		Task<object> LoadByUniqueKey(string propertyName, object uniqueKey, ISessionImplementor session, bool async);

		/// <summary>
		/// Get the property number of the unique key property
		/// </summary>
		int GetPropertyIndex(string propertyName);
	}
}