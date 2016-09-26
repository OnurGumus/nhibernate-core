#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FieldAccessorFixtureAsync
	{
		protected IPropertyAccessor _accessor;
		protected IGetter _getter;
		protected ISetter _setter;
		protected FieldClass _instance;
		/// <summary>
		/// SetUp the local fields for the test cases.
		/// </summary>
		/// <remarks>
		/// Any classes testing out their field access should override this
		/// and setup their FieldClass instance so that whichever field is
		/// going to be reflected upon is initialized to 0.
		/// </remarks>
		[SetUp]
		public virtual void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("field");
			_getter = _accessor.GetGetter(typeof (FieldClass), "Id");
			_setter = _accessor.GetSetter(typeof (FieldClass), "Id");
			_instance = new FieldClass();
			_instance.InitId(0);
		}
	}
}
#endif
