#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

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
		public void ShouldBeMoneyType()
		{
			if (!(Dialect is MsSql2000Dialect))
			{
				Assert.Ignore("This test does not apply to " + Dialect);
			}

			var sqlType = Dialect.GetTypeName(NHibernateUtil.Currency.SqlType);
			Assert.That(sqlType, Is.EqualTo("MONEY"));
		}

		/// <summary>
		/// Test that two decimal fields that are exactly equal are returned
		/// as Equal by the DecimalType.
		/// </summary>
		[Test]
		public void Equals()
		{
			const decimal lhs = 5.6435M;
			const decimal rhs = 5.6435M;
			var type = (CurrencyType)NHibernateUtil.Currency;
			Assert.IsTrue(type.IsEqual(lhs, rhs));
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

		[Test]
		public void UnsavedValue()
		{
			var type = (CurrencyType)NHibernateUtil.Currency;
			object mappedValue = type.StringToObject("0");
			Assert.AreEqual(0m, mappedValue);
			Assert.IsTrue(type.IsEqual(mappedValue, 0m), "'0' in the mapping file should have been converted to a 0m");
		}
	}
}
#endif
