using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate.Util;

namespace NHibernate.Test.SqlTest
{
	public class NullDateUserType : IUserType
	{
		private SqlType[] sqlTypes = new SqlType[1] {SqlTypeFactory.Date};

		public SqlType[] SqlTypes
		{
			get { return sqlTypes; }
		}

		public System.Type ReturnedType
		{
			get { return typeof(DateTime); }
		}

		public new bool Equals(object x, object y)
		{
			return object.Equals(x, y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public Task<object> NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			int ordinal = rs.GetOrdinal(names[0]);
			if (rs.IsDBNull(ordinal))
			{
				return Task.FromResult<object>(DateTime.MinValue);
			}
			else
			{
				return Task.FromResult<object>(rs.GetDateTime(ordinal));
			}
		}

		public Task NullSafeSet(IDbCommand cmd, object value, int index)
		{
			object valueToSet = ((DateTime) value == DateTime.MinValue) ? DBNull.Value : value;
			((IDbDataParameter) cmd.Parameters[index]).Value = valueToSet;
			return TaskHelper.CompletedTask;
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public bool IsMutable
		{
			get { return false; }
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object Disassemble(object value)
		{
			return value;
		}
	}
}