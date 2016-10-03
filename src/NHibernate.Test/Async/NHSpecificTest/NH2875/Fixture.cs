#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2875
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		public const string ForeignKeyName = "FK_NAME_TO_EXPECT_IN_DDL";
		[Test]
		public async Task SpecifiedForeignKeyNameInByCodeMappingIsUsedInGeneratedSchemaAsync()
		{
			var mapper = new ModelMapper();
			// Generates a schema in which a Person record cannot be created unless an Employee
			// with the same primary key value already exists. The Constrained property of the
			// one-to-one mapping is required to create the foreign key constraint on the Person
			// table, and the optional ForeignKey property is used to name it; otherwise a
			// generated name is used
			mapper.Class<Person>(rc =>
			{
				rc.Id(x => x.Id, map => map.Generator(Generators.Foreign<Employee>(p => p.Person)));
				rc.Property(x => x.Name);
				rc.OneToOne(x => x.Employee, map =>
				{
					map.Constrained(true);
					map.ForeignKey(ForeignKeyName);
				}

				);
			}

			);
			mapper.Class<Employee>(rc =>
			{
				rc.Id(x => x.Id);
				rc.OneToOne(x => x.Person, map =>
				{
				}

				);
			}

			);
			var script = new StringBuilder();
			var cfg = new Configuration();
			cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
			await (new SchemaExport(cfg).ExecuteAsync(s => script.AppendLine(s), false, false));
			Assert.That(script.ToString(), Is.StringContaining(string.Format("constraint {0}", ForeignKeyName)));
		}
	}
}
#endif
