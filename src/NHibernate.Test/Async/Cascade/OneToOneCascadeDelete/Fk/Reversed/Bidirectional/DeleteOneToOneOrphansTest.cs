#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Reversed.Bidirectional
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var emp = new Employee{Name = "Julius Caesar"};
					emp.Info = new EmployeeInfo(emp)
					{JobTitle = "Consul"};
					await (s.SaveAsync(emp));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from EmployeeInfo"));
					await (s.DeleteAsync("from Employee"));
					await (tx.CommitAsync());
				}
		}

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

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DeleteOneToOneOrphansTestHbmAsync : DeleteOneToOneOrphansTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"Cascade.OneToOneCascadeDelete.Fk.Reversed.Bidirectional.Mappings.hbm.xml"};
			}
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DeleteOneToOneOrphansTestByCodeAsync : DeleteOneToOneOrphansTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		protected override void AddMappings(Configuration configuration)
		{
			var mapper = new ModelMapper();
			mapper.Class<Employee>(mc =>
			{
				mc.Id(x => x.Id, map =>
				{
					map.Generator(Generators.Increment);
					map.Column("Id");
				}

				);
				mc.ManyToOne<EmployeeInfo>(x => x.Info, map =>
				{
					map.Column("Info_id");
					map.Unique(true);
					map.Cascade(Mapping.ByCode.Cascade.All | Mapping.ByCode.Cascade.DeleteOrphans);
				}

				);
				mc.Property(x => x.Name);
			}

			);
			mapper.Class<EmployeeInfo>(cm =>
			{
				cm.Id(x => x.Id, m =>
				{
					m.Generator(Generators.Increment);
					m.Column("Id");
				}

				);
				cm.OneToOne<Employee>(x => x.EmployeeDetails, map =>
				{
					map.PropertyReference(x => x.Info);
				}

				);
				cm.Property(x => x.JobTitle);
			}

			);
			configuration.AddDeserializedMapping(mapper.CompileMappingForAllExplicitlyAddedEntities(), "TestDomain");
		}
	}
}
#endif
