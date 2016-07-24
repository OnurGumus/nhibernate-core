#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CompositeUserType : ICompositeUserType
	{
		/// <summary>
		/// Retrieve an instance of the mapped class from a DbDataReader. Implementors
		/// should handle possibility of null values.
		/// </summary>
		/// <param name = "dr">DbDataReader</param>
		/// <param name = "names">the column names</param>
		/// <param name = "session"></param>
		/// <param name = "owner">the containing entity</param>
		/// <returns></returns>
		public async Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			var data = new CompositeData();
			data.DataA = (DateTime)await (NHibernateUtil.DateTime.NullSafeGetAsync(dr, new[]{names[0]}, session, owner));
			data.DataB = (DateTime)await (NHibernateUtil.DateTime.NullSafeGetAsync(dr, new[]{names[1]}, session, owner));
			return data;
		}
	}
}
#endif
