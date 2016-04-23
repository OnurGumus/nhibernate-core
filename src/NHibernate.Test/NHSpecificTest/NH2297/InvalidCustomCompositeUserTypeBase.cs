using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2297
{
	public class InvalidNamesCustomCompositeUserType : InvalidCustomCompositeUserTypeBase
	{
		public override string[] PropertyNames
		{
			// This is an invalid return value
			get { return null; }
		}
	}


	public class InvalidTypesCustomCompositeUserType : InvalidCustomCompositeUserTypeBase
	{
		public override Type.IType[] PropertyTypes
		{
			// This is an invalid return value
			get { return null; }
		}
	}


	/// <summary>
	/// An invalid custom user type mapper.
	/// </summary>
	[Serializable]
	public abstract class InvalidCustomCompositeUserTypeBase : ICompositeUserType
	{
		public System.Type ReturnedClass
		{
			get { return typeof (CustomCompositeUserType); }
		}

		public bool IsMutable
		{
			get { return true; }
		}

		public virtual String[] PropertyNames
		{
			get { return new[] {"Value1", "Value2"}; }
		}

		public virtual Type.IType[] PropertyTypes
		{
			get { return new IType[] {NHibernateUtil.String, NHibernateUtil.String}; }
		}

		public object Assemble(object cached, ISessionImplementor session, object owner)
		{
			return DeepCopy(cached);
		}

		public Task<object> Disassemble(Object value, ISessionImplementor session)
		{
			return Task.FromResult(DeepCopy(value));
		}

		public Object DeepCopy(Object a)
		{
			return a;
		}

		public new bool Equals(object x, object y)
		{
			return (x == y) || (x != null && y != null && (x.Equals(y)));
		}

		public Task<object> NullSafeGet(System.Data.IDataReader rs, String[] names, NHibernate.Engine.ISessionImplementor session,
		                          Object owner)
		{
			return NHibernateUtil.String.NullSafeGet(rs, names[0], session, owner);
		}

		public Task NullSafeSet(System.Data.IDbCommand st, Object value, int index,
								 bool[] settable, NHibernate.Engine.ISessionImplementor session)
		{
			return TaskHelper.FromException<object>(new NotImplementedException());
		}

		public Object GetPropertyValue(Object component, int property)
		{
			return null;
		}

		public void SetPropertyValue(Object object1, int i, Object object2)
		{

		}

		public int GetHashCode(object x)
		{
			return x == null ? typeof (string).GetHashCode() : x.GetHashCode();
		}

		public Task<object> Replace(object original, object target, ISessionImplementor session, object owner)
		{
			return Task.FromResult(DeepCopy(original));
		}
	}
}