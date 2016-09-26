#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cfg.XmlHbmBinding;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1007
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestIdGeneratorAttributeMappingOnIdentifierAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var employer = new Employer1();
					Assert.That(employer.Id, Is.EqualTo(Guid.Empty));
					await (session.SaveAsync(employer));
					Assert.That(employer.Id, Is.Not.EqualTo(Guid.Empty));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task MappingIdGeneratorWithAttributeTakesPrecendenceOverMappingWithElementAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var employer = new Employer2();
					Assert.That(employer.Id, Is.EqualTo(Guid.Empty));
					await (session.SaveAsync(employer));
					Assert.That(employer.Id, Is.Not.EqualTo(Guid.Empty));
					await (transaction.CommitAsync());
				}
		}

		private void VerifyMapping(HbmMapping mapping)
		{
			var dialect = new MsSql2008Dialect();
			var configuration = new Configuration();
			var mappings = configuration.CreateMappings(dialect);
			mappings.DefaultAssembly = "NHibernate.Test";
			mappings.DefaultNamespace = "NHibernate.Test.NHSpecificTest.NH1007";
			var rootBinder = new MappingRootBinder(mappings, dialect);
			rootBinder.Bind(mapping);
			var employer = rootBinder.Mappings.GetClass("NHibernate.Test.NHSpecificTest.NH1007.Employer1");
			var simpleValue = employer.Identifier as SimpleValue;
			if (simpleValue != null)
			{
				Assert.That(simpleValue.IdentifierGeneratorStrategy, Is.EqualTo("guid"));
				Assert.That(simpleValue.IdentifierGeneratorProperties, Is.Empty);
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from System.Object").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
