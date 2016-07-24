#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1965
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReattachWithCollectionTestAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			// Note: DeleteOrphans has no sense, only added to match the case reported.
			mapper.Class<Cat>(cm =>
			{
				cm.Id(x => x.Id, map => map.Generator(Generators.Identity));
				cm.Bag(x => x.Children, map => map.Cascade(Mapping.ByCode.Cascade.All.Include(Mapping.ByCode.Cascade.DeleteOrphans)), rel => rel.OneToMany());
			}

			);
			var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
			return mappings;
		}

		[Test]
		public async Task WhenReattachThenNotThrowsAsync()
		{
			var cat = new Cat();
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.SaveAsync(cat));
					await (session.Transaction.CommitAsync());
				}

			using (var session = OpenSession())
			{
				Assert.That(async () => await (session.LockAsync(cat, LockMode.None)), Throws.Nothing);
			}

			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.DeleteAsync(cat));
					await (session.Transaction.CommitAsync());
				}
		}
	}
}
#endif
