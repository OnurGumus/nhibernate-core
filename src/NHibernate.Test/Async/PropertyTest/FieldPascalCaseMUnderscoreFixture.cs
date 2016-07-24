#if NET_4_5
using System;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FieldPascalCaseMUnderscoreFixtureAsync : FieldAccessorFixtureAsync
	{
		[SetUp]
		public override void SetUp()
		{
			_accessor = PropertyAccessorFactory.GetPropertyAccessor("field.pascalcase-m-underscore");
			_getter = _accessor.GetGetter(typeof (FieldClass), "Blah");
			_setter = _accessor.GetSetter(typeof (FieldClass), "Blah");
			_instance = new FieldClass();
			_instance.InitBlah(0);
		}
	}
}
#endif
