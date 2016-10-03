#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2856
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.GenerateStatistics, "true");
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(rc =>
			{
				rc.Table("Person");
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Name);
				rc.ManyToOne(x => x.Address, m => m.Column("AddressId"));
			}

			);
			mapper.Class<Address>(rc =>
			{
				rc.Table("Addresses");
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Name);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var person = new Person{Id = 5, Name = "Joe Bloggs"};
					await (session.SaveAsync(person));
					var address = new Address{Id = 15, Name = "Home"};
					await (session.SaveAsync(address));
					person.Address = address;
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (session.DeleteAsync("from Address"));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
