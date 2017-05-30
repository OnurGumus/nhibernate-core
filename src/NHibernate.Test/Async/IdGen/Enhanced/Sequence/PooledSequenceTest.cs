﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NUnit.Framework;
using NHibernate.Id.Enhanced;

namespace NHibernate.Test.IdGen.Enhanced.Sequence
{
	using System.Threading.Tasks;
	[TestFixture]
	public class PooledSequenceTestAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new[] { "IdGen.Enhanced.Sequence.Pooled.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		[Test]
		public async Task TestNormalBoundaryAsync()
		{
			var persister = sessions.GetEntityPersister(typeof(Entity).FullName);
			Assert.That(persister.IdentifierGenerator, Is.TypeOf<SequenceStyleGenerator>());

			var generator = (SequenceStyleGenerator)persister.IdentifierGenerator;
			Assert.That(generator.Optimizer, Is.TypeOf<OptimizerFactory.PooledOptimizer>());

			var optimizer = (OptimizerFactory.PooledOptimizer)generator.Optimizer;

			int increment = optimizer.IncrementSize;
			var entities = new Entity[increment + 1];

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					for (int i = 0; i < increment; i++)
					{
						entities[i] = new Entity("" + (i + 1));
						await (session.SaveAsync(entities[i]));
						Assert.That(generator.DatabaseStructure.TimesAccessed, Is.EqualTo(2)); // initialization calls seq twice
						Assert.That(optimizer.LastSourceValue, Is.EqualTo(increment + 1)); // initialization calls seq twice
						Assert.That(optimizer.LastValue, Is.EqualTo(i + 1));
					}

					// now force a "clock over"
					entities[increment] = new Entity("" + increment);
					await (session.SaveAsync(entities[increment]));
					Assert.That(generator.DatabaseStructure.TimesAccessed, Is.EqualTo(3)); // initialization (2) + clock over
					Assert.That(optimizer.LastSourceValue, Is.EqualTo(increment * 2 + 1)); // initialization (2) + clock over
					Assert.That(optimizer.LastValue, Is.EqualTo(increment + 1));

					await (transaction.CommitAsync());
				}

				using (ITransaction transaction = session.BeginTransaction())
				{
					for (int i = 0; i < entities.Length; i++)
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