#if NET_4_5
using System.Collections;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Composite
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DeleteOneToOneOrphansTestAsync : TestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var emp = new Employee{Name = "Julius Caesar"};
					emp.Info = new EmployeeInfo(1L, 1L);
					await (s.SaveAsync(emp.Info));
					await (s.SaveAsync(emp));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from EmployeeInfo"));
					await (session.DeleteAsync("from Employee"));
					await (tx.CommitAsync());
				}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

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

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DeleteOneToOneOrphansTestHbmAsync : DeleteOneToOneOrphansTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"Cascade.OneToOneCascadeDelete.Fk.Composite.Mappings.hbm.xml"};
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

		protected override void AddMappings(Cfg.Configuration configuration)
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
					// Columns have to be declared first otherwise other properties are reset.
					map.Columns(x =>
					{
						x.Name("COMP_ID");
					}

					, x =>
					{
						x.Name("PERS_ID");
					}

					);
					map.Unique(true);
					map.Cascade(Mapping.ByCode.Cascade.All | Mapping.ByCode.Cascade.DeleteOrphans);
					map.NotFound(NotFoundMode.Exception);
				}

				);
				mc.Property(x => x.Name);
			}

			);
			mapper.Class<EmployeeInfo>(mc =>
			{
				mc.ComponentAsId<EmployeeInfo.Identifier>(x => x.Id, map =>
				{
					map.Property(x => x.CompanyId, m => m.Column("COMPS_ID"));
					map.Property(x => x.PersonId, m => m.Column("PERS_ID"));
				}

				);
			}

			);
			configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
		}
	}
}
#endif
