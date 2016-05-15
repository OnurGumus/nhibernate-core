#if NET_4_5
using System.Collections;
using NHibernate.Id.Enhanced;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.Enhanced.Table
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicTableTest : TestCase
	{
		[Test]
		public async Task TestNormalBoundaryAsync()
		{
			var persister = sessions.GetEntityPersister(typeof (Entity).FullName);
			Assert.That(persister.IdentifierGenerator, Is.TypeOf<TableGenerator>());
			var generator = (TableGenerator)persister.IdentifierGenerator;
			Assert.That(generator.Optimizer, Is.TypeOf<OptimizerFactory.NoopOptimizer>());
			int count = 5;
			Entity[] entities = new Entity[count];
			using (ISession s = OpenSession())
			{
				using (ITransaction transaction = s.BeginTransaction())
				{
					for (int i = 0; i < count; i++)
					{
						entities[i] = new Entity("" + (i + 1));
						await (s.SaveAsync(entities[i]));
						long expectedId = i + 1;
						Assert.That(entities[i].Id, Is.EqualTo(expectedId));
						Assert.That(generator.TableAccessCount, Is.EqualTo(expectedId));
						Assert.That(generator.Optimizer.LastSourceValue, Is.EqualTo(expectedId));
					}

					await (transaction.CommitAsync());
				}

				using (ITransaction transaction = s.BeginTransaction())
				{
					for (int i = 0; i < count; i++)
					{
						Assert.That(entities[i].Id, Is.EqualTo(i + 1));
						await (s.DeleteAsync(entities[i]));
					}

					await (transaction.CommitAsync());
				}
			}
		}
	}
}
#endif
