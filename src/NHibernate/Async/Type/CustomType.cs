using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomType : AbstractType, IDiscriminatorType, IVersionType
	{
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			return userType.Assemble(cached, owner);
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return userType.NullSafeGet(rs, names, owner);
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return await (NullSafeGetAsync(rs, new string[]{name}, session, owner));
		}

		public override async Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			return userType.Replace(original, current, owner);
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return checkable[0] && await (IsDirtyAsync(old, current, session));
		}

		public override async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			return IsEqual(x, y);
		}

		public override async Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return userType.DeepCopy(value);
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return userType.Disassemble(value);
		}

		public override async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			bool[] result = new bool[GetColumnSpan(mapping)];
			if (value != null)
				ArrayHelper.Fill(result, true);
			return result;
		}

		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			return userType.GetHashCode(x);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name = "value"></param>
		/// <param name = "factory"></param>
		/// <returns></returns>
		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			if (value == null)
			{
				return "null";
			}
			else
			{
				return ToXMLString(value, factory);
			}
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				userType.NullSafeSet(st, value, index);
		}

		public override async Task NullSafeSetAsync(IDbCommand cmd, object value, int index, ISessionImplementor session)
		{
			userType.NullSafeSet(cmd, value, index);
		}

		public async Task<object> SeedAsync(ISessionImplementor session)
		{
			return ((IUserVersionType)userType).Seed(session);
		}

		public async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return ((IUserVersionType)userType).Next(current, session);
		}
	}
}