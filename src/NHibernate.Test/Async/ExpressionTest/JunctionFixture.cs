#if NET_4_5
using NHibernate.DomainModel;
using NHibernate.Engine;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JunctionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task SqlStringAsync()
		{
			SqlString sqlString;
			using (ISession session = factory.OpenSession())
			{
				CreateObjects(typeof (Simple), session);
				sqlString = await (_conjunction.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			}

			string expectedSql = "(sql_alias.address is null and sql_alias.count_ between ? and ?)";
			CompareSqlStrings(sqlString, expectedSql, 2);
		}

		[Test]
		public async Task GetTypedValuesAsync()
		{
			TypedValue[] typedValues;
			using (ISession session = factory.OpenSession())
			{
				CreateObjects(typeof (Simple), session);
				typedValues = await (_conjunction.GetTypedValuesAsync(criteria, criteriaQuery));
			}

			TypedValue[] expectedTV = new TypedValue[2];
			expectedTV[0] = new TypedValue(NHibernateUtil.Int32, 5, EntityMode.Poco);
			expectedTV[1] = new TypedValue(NHibernateUtil.Int32, 10, EntityMode.Poco);
			Assert.AreEqual(2, typedValues.Length);
			for (int i = 0; i < typedValues.Length; i++)
			{
				Assert.AreEqual(expectedTV[i].Type, typedValues[i].Type);
				Assert.AreEqual(expectedTV[i].Value, typedValues[i].Value);
			}
		}
	}
}
#endif
