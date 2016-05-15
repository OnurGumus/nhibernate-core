#if NET_4_5
using System.Linq;
using System.Transactions;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AutoFlushFixture : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldAutoFlushWhenInExplicitTransactionAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var result = (
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e).ToList();
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}

		[Test]
		public async Task ShouldAutoFlushWhenInDistributedTransactionAsync()
		{
			using (new TransactionScope())
				using (var session = OpenSession())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var result = (
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e).ToList();
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
