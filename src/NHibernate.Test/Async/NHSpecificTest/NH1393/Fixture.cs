#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1393
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanSumProjectionOnSqlFunctionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Sum(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(334, list[0]);
			}
		}

		[Test]
		public async Task CanAvgProjectionOnSqlFunctionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Avg(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(((double)334) / 5, list[0]);
			}
		}

		[Test]
		public async Task CanMinProjectionOnIdentityProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Min(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(19, list[0]);
			}
		}

		[Test]
		public async Task CanMaxProjectionOnIdentityProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Max(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(108, list[0]);
			}
		}
	}
}
#endif
