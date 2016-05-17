#if NET_4_5
using System;
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DoubleTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			DoubleClass basic = new DoubleClass();
			basic.Id = 1;
			basic.DoubleValue = _values[0];
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (DoubleClass)s.Load(typeof (DoubleClass), 1);
			Assert.AreEqual(_values[0], basic.DoubleValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
