#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH276.JoinedSubclass
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH276.JoinedSubclass.Mappings.hbm.xml"};
			}
		}

		[Test]
		public async Task ManyToOneIdPropertiesAsync()
		{
			Organization org = new Organization();
			org.OrganizationId = 5;
			org.Name = "the org";
			Status stat = new Status();
			stat.StatusId = 4;
			stat.Name = "the stat";
			Request r = new Request();
			r.Extra = "extra";
			r.Office = org;
			r.Status = stat;
			ISession s = OpenSession();
			await (s.SaveAsync(org));
			await (s.SaveAsync(stat));
			await (s.SaveAsync(r));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof (Request));
			c.Add(Expression.Eq("Status.StatusId", 1));
			c.Add(Expression.Eq("Office.OrganizationId", 1));
			IList list = await (c.ListAsync());
			Assert.AreEqual(0, list.Count, "should contain no results");
			c = s.CreateCriteria(typeof (Request));
			c.Add(Expression.Eq("Status.StatusId", 4));
			c.Add(Expression.Eq("Office.OrganizationId", 5));
			list = await (c.ListAsync());
			Assert.AreEqual(1, list.Count, "one matching result");
			r = list[0] as Request;
			await (s.DeleteAsync(r));
			await (s.DeleteAsync(r.Status));
			await (s.DeleteAsync(r.Office));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
