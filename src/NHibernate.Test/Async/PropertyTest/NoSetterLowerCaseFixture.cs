#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NoSetterLowerCaseFixtureAsync : NoSetterAccessorFixture
	{
		[SetUp]
		public override void SetUp()
		{
			_expectedLowerFooGetterCalled = true;
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("nosetter.lowercase");
			_getter = _accessor.GetGetter(typeof (FieldClass), "LowerFoo");
			_setter = _accessor.GetSetter(typeof (FieldClass), "LowerFoo");
			_instance = new FieldClass();
			_instance.InitLowerFoo(0);
		}
	}
}
#endif
