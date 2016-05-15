#if NET_4_5
using System;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.DomainModel
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiplicityType : ICompositeUserType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader rs, String[] names, ISessionImplementor session, Object owner)
		{
			int c = (int)await (NHibernateUtil.Int32.NullSafeGetAsync(rs, names[0], session, owner));
			GlarchProxy g = (GlarchProxy)await (NHibernateUtil.Entity(typeof (Glarch)).NullSafeGetAsync(rs, names[1], session, owner));
			Multiplicity m = new Multiplicity();
			m.count = (c == 0 ? 0 : c);
			m.glarch = g;
			return m;
		}

		public async Task NullSafeSetAsync(DbCommand st, Object value, int index, bool[] settable, ISessionImplementor session)
		{
			Multiplicity o = (Multiplicity)value;
			GlarchProxy g;
			int c;
			if (o == null)
			{
				g = null;
				c = 0;
			}
			else
			{
				g = o.glarch;
				c = o.count;
			}

			if (settable[0])
				await (NHibernateUtil.Int32.NullSafeSetAsync(st, c, index, session));
			await (NHibernateUtil.Entity(typeof (Glarch)).NullSafeSetAsync(st, g, index + 1, settable.Skip(1).ToArray(), session));
		}

		public async Task<object> AssembleAsync(object cached, ISessionImplementor session, Object owner)
		{
			if (cached == null)
			{
				return null;
			}

			object[] o = (object[])cached;
			Multiplicity m = new Multiplicity();
			m.count = (int)o[0];
			m.glarch = o[1] == null ? null : (GlarchProxy)await (session.InternalLoadAsync(typeof (Glarch).FullName, o[1], false, false));
			return m;
		}

		public async Task<object> DisassembleAsync(Object value, ISessionImplementor session)
		{
			if (value == null)
			{
				return null;
			}

			Multiplicity m = (Multiplicity)value;
			return new object[]{m.count, await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(null, m.glarch, session))};
		}

		public async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner)
		{
			return await (AssembleAsync(await (DisassembleAsync(original, session)), session, owner));
		}
	}
}
#endif
