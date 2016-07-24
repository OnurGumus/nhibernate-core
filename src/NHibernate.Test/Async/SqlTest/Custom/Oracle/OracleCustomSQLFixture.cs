#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom.Oracle
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OracleCustomSQLFixtureAsync : CustomStoredProcSupportTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"SqlTest.Custom.Oracle.Mappings.hbm.xml", "SqlTest.Custom.Oracle.StoredProcedures.hbm.xml"};
			}
		}

		protected override bool AppliesTo(NHibernate.Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver is Driver.OracleDataClientDriver;
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Oracle8iDialect;
		}

		[Test]
		public async Task RefCursorOutStoredProcedureAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Organization ifa = new Organization("IFA");
			Organization jboss = new Organization("JBoss");
			Person gavin = new Person("Gavin");
			Person kevin = new Person("Kevin");
			Employment emp = new Employment(gavin, jboss, "AU");
			Employment emp2 = new Employment(kevin, ifa, "EU");
			await (s.SaveAsync(ifa));
			await (s.SaveAsync(jboss));
			await (s.SaveAsync(gavin));
			await (s.SaveAsync(kevin));
			await (s.SaveAsync(emp));
			await (s.SaveAsync(emp2));
			IQuery namedQuery = s.GetNamedQuery("selectEmploymentsForRegion");
			namedQuery.SetString("regionCode", "EU");
			IList list = await (namedQuery.ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			await (s.DeleteAsync(emp2));
			await (s.DeleteAsync(emp));
			await (s.DeleteAsync(ifa));
			await (s.DeleteAsync(jboss));
			await (s.DeleteAsync(kevin));
			await (s.DeleteAsync(gavin));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
