#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2394
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	partial class PhoneNumberUserType : ICompositeUserType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			if (await (dr.IsDBNullAsync(dr.GetOrdinal(names[0]))))
				return null;
			return new PhoneNumber((int)await (NHibernateUtil.Int32.NullSafeGetAsync(dr, names[0], session, owner)), (string)await (NHibernateUtil.String.NullSafeGetAsync(dr, names[1], session, owner)));
		}

		public async Task NullSafeSetAsync(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
		{
			object countryCode = value == null ? null : (int ? )((PhoneNumber)value).CountryCode;
			object number = value == null ? null : ((PhoneNumber)value).Number;
			if (settable[0])
				await (NHibernateUtil.Int32.NullSafeSetAsync(cmd, countryCode, index++, session));
			if (settable[1])
				await (NHibernateUtil.String.NullSafeSetAsync(cmd, number, index, session));
		}

		public Task<object> DisassembleAsync(object value, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Disassemble(value, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Assemble(cached, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Replace(original, target, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
