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
	public partial class FieldSetterExceptionFixtureAsync
	{
		protected IPropertyAccessor _accessor;
		protected ISetter _setter;
		[SetUp]
		public void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("field");
			_setter = _accessor.GetSetter(typeof (A), "Id");
		}

		[Test]
		public Task SetInvalidTypeAsync()
		{
			try
			{
				A instance = new A();
				var e = Assert.Throws<PropertyAccessException>(() => _setter.Set(instance, "wrong type"));
				Assert.That(e.Message, Is.EqualTo("The type System.String can not be assigned to a field of type System.Int32 setter of NHibernate.Test.PropertyTest.FieldSetterExceptionFixture+A.Id"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class A
		{
			public int Id = 0;
		}
	}
}
#endif
