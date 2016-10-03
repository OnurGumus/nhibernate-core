#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using NHibernate.Id.Enhanced;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class IdentifierGeneratorFactory
	{
		/// <summary> Get the generated identifier when using identity columns</summary>
		/// <param name = "rs">The <see cref = "DbDataReader"/> to read the identifier value from.</param>
		/// <param name = "type">The <see cref = "IIdentifierType"/> the value should be converted to.</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> the value is retrieved in.</param>
		/// <returns> The value for the identifier. </returns>
		public static async Task<object> GetGeneratedIdentityAsync(DbDataReader rs, IType type, ISessionImplementor session)
		{
			if (!await (rs.ReadAsync()))
			{
				throw new HibernateException("The database returned no natively generated identity value");
			}

			object id = await (GetAsync(rs, type, session));
			if (log.IsDebugEnabled)
			{
				log.Debug("Natively generated identity: " + id);
			}

			return id;
		}

		/// <summary>
		/// Gets the value of the identifier from the <see cref = "DbDataReader"/> and
		/// ensures it is the correct <see cref = "System.Type"/>.
		/// </summary>
		/// <param name = "rs">The <see cref = "DbDataReader"/> to read the identifier value from.</param>
		/// <param name = "type">The <see cref = "IIdentifierType"/> the value should be converted to.</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> the value is retrieved in.</param>
		/// <returns>
		/// The value for the identifier.
		/// </returns>
		/// <exception cref = "IdentifierGenerationException">
		/// Thrown if there is any problem getting the value from the <see cref = "DbDataReader"/>
		/// or with converting it to the <see cref = "System.Type"/>.
		/// </exception>
		public static async Task<object> GetAsync(DbDataReader rs, IType type, ISessionImplementor session)
		{
			// here is an interesting one: 
			// - MsSql's @@identity returns a Decimal
			// - MySql LAST_IDENITY() returns an Int64 			
			try
			{
				return await (type.NullSafeGetAsync(rs, rs.GetName(0), session, null));
			}
			catch (Exception e)
			{
				throw new IdentifierGenerationException("could not retrieve identifier value", e);
			}
		}
	}
}
#endif
