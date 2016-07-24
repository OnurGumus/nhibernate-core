#if NET_4_5
using System.Collections;
using NHibernate.Id.Enhanced;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.Enhanced.Table
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PooledLoTableTestAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"IdGen.Enhanced.Table.PooledLo.hbm.xml"};
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
		public async Task TestNormalBoundaryAsync()
		{
			var persister = sessions.GetEntityPersister(typeof (Entity).FullName);
			Assert.That(persister.IdentifierGenerator, Is.TypeOf<TableGenerator>());
			var generator = (TableGenerator)persister.IdentifierGenerator;
			Assert.That(generator.Optimizer, Is.TypeOf<OptimizerFactory.PooledLoOptimizer>());
			var optimizer = (OptimizerFactory.PooledLoOptimizer)generator.Optimizer;
			int increment = optimizer.IncrementSize;
			Entity[] entities = new Entity[increment + 1];
			using (ISession s = OpenSession())
			{
				using (ITransaction transaction = s.BeginTransaction())
				{
					for (int i = 0; i < increment; i++)
					{
						entities[i] = new Entity("" + (i + 1));
						await (s.SaveAsync(entities[i]));
						Assert.That(generator.TableAccessCount, Is.EqualTo(1)); // initialization
						Assert.That(optimizer.LastSourceValue, Is.EqualTo(1)); // initialization
						Assert.That(optimizer.LastValue, Is.EqualTo(i + 1));
					}

					// now force a "clock over"
					entities[increment] = new Entity("" + increment);
					await (s.SaveAsync(entities[increment]));
					Assert.That(generator.TableAccessCount, Is.EqualTo(2));
					Assert.That(optimizer.LastSourceValue, Is.EqualTo(increment + 1));
					Assert.That(optimizer.LastValue, Is.EqualTo(increment + 1));
					await (transaction.CommitAsync());
				}

				using (ITransaction transaction = s.BeginTransaction())
				{
					for (int i = 0; i < entities.Length; i++)
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
