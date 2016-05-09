using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using NHibernate.Id.Enhanced;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// Factory methods for <c>IdentifierGenerator</c> framework.
	/// </summary>
	/// <remarks>
	/// <p>The built in strategies for identifier generation in NHibernate are:</p>
	/// <list type = "table">
	///		<listheader>
	///			<term>strategy</term>
	///			<description>Implementation of strategy</description>
	///		</listheader>
	///		<item>
	///			<term>assigned</term>
	///			<description><see cref = "Assigned"/></description>
	///		</item>
	///		<item>
	///			<term>counter</term>
	///			<description><see cref = "CounterGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>foreign</term>
	///			<description><see cref = "ForeignGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>guid</term>
	///			<description><see cref = "GuidGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>guid.comb</term>
	///			<description><see cref = "GuidCombGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>guid.native</term>
	///			<description><see cref = "NativeGuidGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>hilo</term>
	///			<description><see cref = "TableHiLoGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>identity</term>
	///			<description><see cref = "IdentityGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>native</term>
	///			<description>
	///				Chooses between <see cref = "IdentityGenerator"/>, <see cref = "SequenceGenerator"/>
	///				, and <see cref = "TableHiLoGenerator"/> based on the 
	///				<see cref = "Dialect.Dialect"/>'s capabilities.
	///			</description>
	///		</item>
	///		<item>
	///			<term>seqhilo</term>
	///			<description><see cref = "SequenceHiLoGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>sequence</term>
	///			<description><see cref = "SequenceGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>enhanced-sequence</term>
	///			<description><see cref = "SequenceStyleGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>uuid.hex</term>
	///			<description><see cref = "UUIDHexGenerator"/></description>
	///		</item>
	///		<item>
	///			<term>uuid.string</term>
	///			<description><see cref = "UUIDStringGenerator"/></description>
	///		</item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class IdentifierGeneratorFactory
	{
		public static async Task<object> GetAsync(IDataReader rs, IType type, ISessionImplementor session)
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

		public static async Task<object> GetGeneratedIdentityAsync(IDataReader rs, IType type, ISessionImplementor session)
		{
			if (!rs.Read())
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
	}
}