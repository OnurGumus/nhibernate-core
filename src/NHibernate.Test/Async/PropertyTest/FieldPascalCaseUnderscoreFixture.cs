#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FieldPascalCaseUnderscoreFixtureAsync : FieldAccessorFixtureAsync
	{
		[SetUp]
		public override void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("field.pascalcase-underscore");
			_getter = _accessor.GetGetter(typeof (FieldClass), "PascalUnderscoreFoo");
			_setter = _accessor.GetSetter(typeof (FieldClass), "PascalUnderscoreFoo");
			_instance = new FieldClass();
			_instance.InitPascalUnderscoreFoo(0);
		}
	}
}
#endif
