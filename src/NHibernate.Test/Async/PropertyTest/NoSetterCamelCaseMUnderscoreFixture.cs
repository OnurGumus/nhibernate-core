#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NoSetterCamelCaseMUnderscoreFixtureAsync : NoSetterAccessorFixture
	{
		[SetUp]
		public override void SetUp()
		{
			_expectedCamelMUnderscoreGetterCalled = true;
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("nosetter.camelcase-m-underscore");
			_getter = _accessor.GetGetter(typeof (FieldClass), "CamelMUnderscore");
			_setter = _accessor.GetSetter(typeof (FieldClass), "CamelMUnderscore");
			_instance = new FieldClass();
			_instance.InitCamelCaseMUnderscore(0);
		}
	}
}
#endif
