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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JunctionFixtureAsync : BaseExpressionFixture
	{
		private Conjunction _conjunction;
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			_conjunction = Expression.Conjunction();
			_conjunction.Add(Expression.IsNull("Address")).Add(Expression.Between("Count", 5, 10));
		}

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

		[Test]
		public void ToStringTest()
		{
			Assert.AreEqual("(Address is null and Count between 5 and 10)", _conjunction.ToString());
		}
	}
}
#endif
