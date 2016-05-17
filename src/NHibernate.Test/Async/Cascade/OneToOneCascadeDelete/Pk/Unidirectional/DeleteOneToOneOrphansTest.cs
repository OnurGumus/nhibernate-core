#if NET_4_5
using System.Collections;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Pk.Unidirectional
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTest : TestCase
	{
		[Test]
		public async Task TestOrphanedWhileManagedAsync()
		{
			long empId;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var empInfoList = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(1, empInfoList.Count);
					var empList = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empList.Count);
					Employee emp = empList[0];
					Assert.NotNull(emp.Info);
					var empAndInfoList = s.CreateQuery("from Employee e, EmployeeInfo i where e.Info = i").List();
					Assert.AreEqual(1, empAndInfoList.Count);
					var result = (object[])empAndInfoList[0];
					emp = result[0] as Employee;
					Assert.NotNull(result[1]);
					Assert.AreSame(emp.Info, result[1]);
					empId = emp.Id;
					emp.Info = null;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var emp = await (s.GetAsync<Employee>(empId));
					Assert.IsNull(emp.Info);
					var empInfoList = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(0, empInfoList.Count);
					var empList = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empList.Count);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
