#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CurrencyTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			const decimal expected = 5.6435M;
			var basic = new CurrencyClass{CurrencyValue = expected};
			ISession s = OpenSession();
			object savedId = await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = await (s.LoadAsync<CurrencyClass>(savedId));
			Assert.AreEqual(expected, basic.CurrencyValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
