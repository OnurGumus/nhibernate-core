#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CurrencyTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Currency";
			}
		}

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
