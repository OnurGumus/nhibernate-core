#if NET_4_5
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3070
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override Cfg.MappingSchema.HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Employee>(ca =>
			{
				ca.Id(x => x.Id, map =>
				{
					map.Column("Id");
					map.Generator(Generators.Identity);
				}

				);
				ca.Property(x => x.FirstName, map =>
				{
					map.Formula("(select 'something')");
					map.Lazy(true);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public async Task ProxyForEntityWithLazyPropertiesAndFormulaShouldEqualItselfAsync()
		{
			try
			{
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						var emp = new Employee();
						await (s.SaveAsync(emp));
						await (t.CommitAsync());
					}

				using (var session = OpenSession())
				{
					var emps = await (session.QueryOver<Employee>().ListAsync());
					var emp = emps[0];
					// THIS ASSERT WILL FAIL 
					Assert.IsTrue(emp.Equals(emp), "Equals");
				}
			}
			finally
			{
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						await (s.DeleteAsync("from Employee"));
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
