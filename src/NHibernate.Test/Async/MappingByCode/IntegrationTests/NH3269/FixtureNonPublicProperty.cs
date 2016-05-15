#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3269
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureNonPublicProperty : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldThrowExceptionWhenTryingToSaveInherited1WithDuplicateNameAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Inherited1{Name = "Bob"};
					await (session.SaveAsync(e1));
					Assert.That(() =>
					{
						transaction.Commit();
					}

					, Throws.Exception);
				}
		}

		[Test]
		public async Task ShouldNotThrowExceptionWhenTryingToSaveInherited2WithDuplicateNameAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e2 = new Inherited2{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
