#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Bidirectional
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTest : TestCase
	{
		[Test]
		public async Task TestOrphanedWhileManagedAsync()
		{
			long empId;
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var empInfoResults = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(1, empInfoResults.Count);
					var empResults = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empResults.Count);
					var emp = empResults[0];
					Assert.NotNull(emp);
					empId = emp.Id;
					emp.Info = null;
					await (t.CommitAsync());
				}

			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var emp = await (s.GetAsync<Employee>(empId));
					Assert.Null(emp.Info);
					var empInfoResults = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(0, empInfoResults.Count);
					var empResults = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empResults.Count);
					await (t.CommitAsync());
				}
		}
	}
}
#endif
