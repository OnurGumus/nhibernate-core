#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cfg.XmlHbmBinding;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1007
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
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
	}
}
#endif
