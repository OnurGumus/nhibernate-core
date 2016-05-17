#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Reversed.Unidirectional
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTest : TestCase
	{
		[Test]
		public async Task TestOrphanedWhileManagedAsync()
		{
			long empId = 0;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var empInfoList = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(1, empInfoList.Count);
					var empList = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empList.Count);
					Employee emp = empList[0];
					Assert.NotNull(emp.Info);
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

		[Test]
		public async Task TestOrphanedWhileDetachedAsync()
		{
			long empId = 0;
			Employee emp;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var empInfoList = s.CreateQuery("from EmployeeInfo").List<EmployeeInfo>();
					Assert.AreEqual(1, empInfoList.Count);
					var empList = s.CreateQuery("from Employee").List<Employee>();
					Assert.AreEqual(1, empList.Count);
					emp = empList[0];
					Assert.NotNull(emp.Info);
					empId = emp.Id;
					await (tx.CommitAsync());
				}

			//only fails if the object is detached
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					s.Lock(emp, LockMode.None);
					emp.Info = null;
					//save using the new session (this used to work prior to 3.5.x)
					s.SaveOrUpdate(emp);
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					emp = await (s.GetAsync<Employee>(emp.Id));
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
