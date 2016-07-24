#if NET_4_5
using NUnit.Framework;
using NHibernate.Id.Enhanced;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.IdGen.Enhanced.Forcedtable
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicForcedTableSequenceTestAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"IdGen.Enhanced.Forcedtable.Basic.hbm.xml"};
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
			Assert.That(persister.IdentifierGenerator, Is.TypeOf<SequenceStyleGenerator>());
			var generator = (SequenceStyleGenerator)persister.IdentifierGenerator;
			Assert.That(generator.DatabaseStructure, Is.TypeOf<TableStructure>());
			Assert.That(generator.Optimizer, Is.TypeOf<OptimizerFactory.NoopOptimizer>());
			const int count = 5;
			var entities = new Entity[5];
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					for (int i = 0; i < count; i++)
					{
						entities[i] = new Entity("" + (i + 1));
						await (session.SaveAsync(entities[i]));
						long expectedId = i + 1;
						Assert.That(entities[i].Id, Is.EqualTo(expectedId));
						Assert.That(generator.DatabaseStructure.TimesAccessed, Is.EqualTo(expectedId));
						Assert.That(generator.Optimizer.LastSourceValue, Is.EqualTo(expectedId));
					}

					await (transaction.CommitAsync());
				}

				using (ITransaction transaction = session.BeginTransaction())
				{
					for (int i = 0; i < count; i++)
					{
						Assert.That(entities[i].Id, Is.EqualTo(i + 1));
						await (session.DeleteAsync(entities[i]));
					}

					await (transaction.CommitAsync());
				}

				session.Close();
			}
		}
	}
}
#endif
