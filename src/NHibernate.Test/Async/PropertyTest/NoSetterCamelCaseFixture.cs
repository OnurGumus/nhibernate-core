#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NoSetterCamelCaseFixtureAsync : NoSetterAccessorFixture
	{
		[SetUp]
		public override void SetUp()
		{
			_expectedCamelBazGetterCalled = true;
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("nosetter.camelcase");
			_getter = _accessor.GetGetter(typeof (FieldClass), "CamelBaz");
			_setter = _accessor.GetSetter(typeof (FieldClass), "CamelBaz");
			_instance = new FieldClass();
			_instance.InitCamelBaz(0);
		}
	}
}
#endif
