#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StringTypeWithLengthFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "String";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{String.Format("TypesTest.{0}ClassWithLength.hbm.xml", TypeName)};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// this test only works where the driver has set an explicit length on the DbParameter
			return dialect is MsSql2008Dialect;
		}

		[Test]
		public async Task NhThrowsOnTooLongAsync()
		{
			int maxStringLength = 4000;
			PropertyValueException ex = Assert.ThrowsAsync<PropertyValueException>(async () =>
			{
				using (ISession s = OpenSession())
				{
					StringClass b = new StringClass();
					b.LongStringValue = new string ('x', maxStringLength + 1);
					await (s.SaveAsync(b));
					await (s.FlushAsync());
				}
			}

			);
			Assert.That(ex.Message, Iz.EqualTo("Error dehydrating property value for NHibernate.Test.TypesTest.StringClass.LongStringValue"));
			Assert.That(ex.InnerException, Iz.TypeOf<HibernateException>());
			Assert.That(ex.InnerException.Message, Iz.EqualTo("The length of the string value exceeds the length configured in the mapping/parameter."));
		}

		[Test]
		public async Task DbThrowsOnTooLongAsync()
		{
			bool dbThrewError = false;
			try
			{
				using (ISession s = OpenSession())
				{
					StringClass b = new StringClass();
					b.StringValue = "0123456789a";
					await (s.SaveAsync(b));
					await (s.FlushAsync());
				}
			}
			catch
			{
				dbThrewError = true;
			}

			Assert.That(dbThrewError, "Database did not throw an error when trying to put too large a value into a column");
		}

		[Test]
		public async Task CriteriaLikeParameterCanExceedColumnSizeAsync()
		{
			if (!(sessions.ConnectionProvider.Driver is SqlClientDriver))
				Assert.Ignore("This test fails against the ODBC driver.  The driver would need to be override to allow longer parameter sizes than the column.");
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new StringClass()
					{Id = 1, StringValue = "AAAAAAAAAB"}));
					await (s.SaveAsync(new StringClass()
					{Id = 2, StringValue = "BAAAAAAAAA"}));
					var aaItems = await (s.CreateCriteria<StringClass>().Add(Restrictions.Like("StringValue", "%AAAAAAAAA%")).ListAsync());
					Assert.That(aaItems.Count, Is.EqualTo(2));
				}
		}

		[Test]
		public async Task HqlLikeParameterCanExceedColumnSizeAsync()
		{
			if (!(sessions.ConnectionProvider.Driver is SqlClientDriver))
				Assert.Ignore("This test fails against the ODBC driver.  The driver would need to be override to allow longer parameter sizes than the column.");
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new StringClass()
					{Id = 1, StringValue = "AAAAAAAAAB"}));
					await (s.SaveAsync(new StringClass()
					{Id = 2, StringValue = "BAAAAAAAAA"}));
					var aaItems = await (s.CreateQuery("from StringClass s where s.StringValue like :likeValue").SetParameter("likeValue", "%AAAAAAAAA%").ListAsync());
					Assert.That(aaItems.Count, Is.EqualTo(2));
				}
		}
	}
}
#endif
