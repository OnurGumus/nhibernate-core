#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DecimalTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			decimal expected = 5.64351M;
			DecimalClass basic = new DecimalClass();
			basic.Id = 1;
			basic.DecimalValue = expected;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (DecimalClass)s.Load(typeof (DecimalClass), 1);
			Assert.AreEqual(expected, basic.DecimalValue);
			Assert.AreEqual(5.643510M, basic.DecimalValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
