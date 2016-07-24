#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FieldLowerCaseUnderscoreFixtureAsync : FieldAccessorFixtureAsync
	{
		[SetUp]
		public override void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("field.lowercase-underscore");
			_getter = _accessor.GetGetter(typeof (FieldClass), "LowerUnderscoreFoo");
			_setter = _accessor.GetSetter(typeof (FieldClass), "LowerUnderscoreFoo");
			_instance = new FieldClass();
			_instance.InitLowerUnderscoreFoo(0);
		}
	}
}
#endif
