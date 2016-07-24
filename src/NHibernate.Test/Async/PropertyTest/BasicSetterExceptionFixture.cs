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
	public partial class BasicSetterExceptionFixtureAsync
	{
		protected IPropertyAccessor _accessor;
		protected ISetter _setter;
		[SetUp]
		public void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("property");
			_setter = _accessor.GetSetter(typeof (A), "Id");
		}

		[Test]
		public Task SetInvalidTypeAsync()
		{
			try
			{
				A instance = new A();
				var e = Assert.Throws<PropertyAccessException>(() => _setter.Set(instance, "wrong type"));
				Assert.That(e.Message, Is.EqualTo("The type System.String can not be assigned to a property of type System.Int32 setter of NHibernate.Test.PropertyTest.BasicSetterExceptionFixture+A.Id"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task SetValueArgumentExceptionAsync()
		{
			try
			{
				A instance = new A();
				// this will throw a TargetInvocationException that gets wrapped in a PropertyAccessException
				var e = Assert.Throws<PropertyAccessException>(() => _setter.Set(instance, 5));
				Assert.That(e.Message, Is.EqualTo("could not set a property value by reflection setter of NHibernate.Test.PropertyTest.BasicSetterExceptionFixture+A.Id"));
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
			private int _id = 0;
			public int Id
			{
				get
				{
					return _id;
				}

				set
				{
					if (value == 5)
					{
						throw new ArgumentException("can't be 5 for testing purposes");
					}

					_id = value;
				}
			}
		}
	}
}
#endif
