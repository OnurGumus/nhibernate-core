#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Reversed.Bidirectional
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
					var empInfoList = await (s.CreateQuery("from EmployeeInfo").ListAsync<EmployeeInfo>());
					Assert.AreEqual(1, empInfoList.Count);
					var empList = await (s.CreateQuery("from Employee").ListAsync<Employee>());
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
					var empInfoList = await (s.CreateQuery("from EmployeeInfo").ListAsync<EmployeeInfo>());
					Assert.AreEqual(0, empInfoList.Count);
					var empList = await (s.CreateQuery("from Employee").ListAsync<Employee>());
					Assert.AreEqual(1, empList.Count);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
