#if NET_4_5
using System.Collections;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Composite
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTest : TestCase
	{
		[Test]
		public async Task TestOrphanedWhileManagedAsync()
		{
			long empId = 0;
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var infoList = await (s.CreateQuery("from EmployeeInfo").ListAsync<EmployeeInfo>());
					Assert.AreEqual(1, infoList.Count);
					var empList = await (s.CreateQuery("from Employee").ListAsync<Employee>());
					Assert.AreEqual(1, empList.Count);
					var emp = empList[0];
					Assert.NotNull(emp.Info);
					empId = emp.Id;
					emp.Info = null;
					await (s.UpdateAsync(emp));
					await (t.CommitAsync());
				}

			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var emp = await (s.GetAsync<Employee>(empId));
					Assert.IsNull(emp.Info);
					var empInfoList = await (s.CreateQuery("from EmployeeInfo").ListAsync<EmployeeInfo>());
					Assert.AreEqual(0, empInfoList.Count);
					var empList = await (s.CreateQuery("from Employee").ListAsync<Employee>());
					Assert.AreEqual(1, empList.Count);
				}
		}
	}
}
#endif
