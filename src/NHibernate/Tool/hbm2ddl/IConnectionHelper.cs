using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// Contract for delegates responsible for managing connection used by the hbm2ddl tools.
	/// </summary>
	public interface IConnectionHelper
	{
		/// <summary>
		///  Prepare the helper for use.
		/// </summary>
		void Prepare();

		/// <summary>
		///  Prepare the helper for use.
		/// </summary>
		Task PrepareAsync();

		/// <summary>
		/// Get a reference to the connection we are using.
		/// </summary>
		DbConnection Connection { get;}

		/// <summary>
		/// Release any resources held by this helper.
		/// </summary>
		void Release();
	}

}
