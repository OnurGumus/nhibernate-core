using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1907
{
	public class MyType
	{
		public int ToPersist { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			var other = (MyType)obj;
			return ToPersist == other.ToPersist;
		}

		public override int GetHashCode()
		{
			return ToPersist.GetHashCode();
		}
	}

	public class SimpleCustomType : IUserType
	{
		private static readonly SqlType[] ReturnSqlTypes = { SqlTypeFactory.Int32 };


		#region IUserType Members

		public new bool Equals(object x, object y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}
			if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
			{
				return false;
			}

			return x.Equals(y);
		}

		public int GetHashCode(object x)
		{
			return (x == null) ? 0 : x.GetHashCode();
		}

		public SqlType[] SqlTypes
		{
			get { return ReturnSqlTypes; }
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public Task NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null)
			{
				((IDbDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
			}
			else
			{
				((IDbDataParameter)cmd.Parameters[index]).Value = ((MyType)value).ToPersist;
			}
			return TaskHelper.CompletedTask;
		}

		public System.Type ReturnedType
		{
			get { return typeof(Int32); }
		}

		public Task<object> NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			int index0 = rs.GetOrdinal(names[0]);
			if (rs.IsDBNull(index0))
			{
				return null;
			}
			int value = rs.GetInt32(index0);
			return Task.FromResult<object>(new MyType { ToPersist = value});
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

		#endregion
	}

}
