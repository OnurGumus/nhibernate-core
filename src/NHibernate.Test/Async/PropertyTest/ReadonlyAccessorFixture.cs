#if NET_4_5
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadonlyAccessorFixtureAsync
	{
		[Test]
		public void GetValue()
		{
			var accessor = PropertyAccessorFactory.GetPropertyAccessor("readonly");
			var getter = accessor.GetGetter(typeof (Calculation), "Sum");
			Assert.That(getter.Get(new Calculation()), Is.EqualTo(2));
		}

		[Test]
		public void SetValue()
		{
			var accessor = PropertyAccessorFactory.GetPropertyAccessor("readonly");
			var getter = accessor.GetGetter(typeof (Calculation), "Sum");
			var setter = accessor.GetSetter(typeof (Calculation), "Sum");
			var i = new Calculation();
			Assert.That(getter.Get(i), Is.EqualTo(2));
			setter.Set(i, 1);
			Assert.That(getter.Get(i), Is.EqualTo(2));
		}
	}
}
#endif
