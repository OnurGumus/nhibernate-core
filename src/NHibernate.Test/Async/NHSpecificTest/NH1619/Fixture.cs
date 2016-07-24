#if NET_4_5
using NHibernate.Dialect;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1619
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1619";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is PostgreSQLDialect;
		}

		[Test]
		public async Task SavingAndRetrievingAsync()
		{
			var entity = new Dude{BooleanValue = true};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
					Assert.AreEqual(true, (await (s.CreateQuery("from Dude").UniqueResultAsync<Dude>())).BooleanValue);
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(entity));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public void UsingBooleanPostgreSQLType()
		{
			Assert.AreEqual("boolean", Dialect.GetTypeName(SqlTypeFactory.Boolean));
		}
	}
}
#endif
