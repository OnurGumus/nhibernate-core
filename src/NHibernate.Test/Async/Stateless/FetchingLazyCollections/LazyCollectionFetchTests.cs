#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Stateless.FetchingLazyCollections
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyCollectionFetchTests : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldWorkLoadingComplexEntitiesAsync()
		{
			const string crocodileFather = "Crocodile father";
			const string crocodileMother = "Crocodile mother";
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var rf = new Reptile{Description = crocodileFather};
					var rm = new Reptile{Description = crocodileMother};
					var rc1 = new Reptile{Description = "Crocodile"};
					var rc2 = new Reptile{Description = "Crocodile"};
					var rfamily = new Family<Reptile>{Father = rf, Mother = rm, Childs = new HashSet<Reptile>{rc1, rc2}};
					s.Save("ReptileFamily", rfamily);
					await (tx.CommitAsync());
				}

			const string humanFather = "Fred";
			const string humanMother = "Wilma";
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var hf = new Human{Description = "Flinstone", Name = humanFather};
					var hm = new Human{Description = "Flinstone", Name = humanMother};
					var hc1 = new Human{Description = "Flinstone", Name = "Pebbles"};
					var hfamily = new Family<Human>{Father = hf, Mother = hm, Childs = new HashSet<Human>{hc1}};
					s.Save("HumanFamily", hfamily);
					await (tx.CommitAsync());
				}

			using (IStatelessSession s = sessions.OpenStatelessSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Family<Human>> hf = s.CreateQuery("from HumanFamily").List<Family<Human>>();
					Assert.That(hf.Count, Is.EqualTo(1));
					Assert.That(hf[0].Father.Name, Is.EqualTo(humanFather));
					Assert.That(hf[0].Mother.Name, Is.EqualTo(humanMother));
					Assert.That(NHibernateUtil.IsInitialized(hf[0].Childs), Is.False, "Lazy collection should NOT be initialized");
					IList<Family<Reptile>> rf = s.CreateQuery("from ReptileFamily").List<Family<Reptile>>();
					Assert.That(rf.Count, Is.EqualTo(1));
					Assert.That(rf[0].Father.Description, Is.EqualTo(crocodileFather));
					Assert.That(rf[0].Mother.Description, Is.EqualTo(crocodileMother));
					Assert.That(NHibernateUtil.IsInitialized(hf[0].Childs), Is.False, "Lazy collection should NOT be initialized");
					await (tx.CommitAsync());
				}

			using (IStatelessSession s = sessions.OpenStatelessSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Family<Human>> hf = s.Query<Family<Human>>().FetchMany(f => f.Childs).ToList();
					Assert.That(hf.Count, Is.EqualTo(1));
					Assert.That(hf[0].Father.Name, Is.EqualTo(humanFather));
					Assert.That(hf[0].Mother.Name, Is.EqualTo(humanMother));
					var initialized1 = NHibernateUtil.IsInitialized(hf[0].Childs);
					Assert.That(initialized1, Is.True, "Lazy collection should be initialized");
					IList<Family<Reptile>> rf = s.Query<Family<Reptile>>().FetchMany(f => f.Childs).ToList();
					Assert.That(rf.Count, Is.EqualTo(1));
					Assert.That(rf[0].Father.Description, Is.EqualTo(crocodileFather));
					Assert.That(rf[0].Mother.Description, Is.EqualTo(crocodileMother));
					var initialized2 = NHibernateUtil.IsInitialized(hf[0].Childs);
					Assert.That(initialized2, Is.True, "Lazy collection should be initialized");
					await (tx.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from HumanFamily"));
					await (s.DeleteAsync("from ReptileFamily"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
