#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2969
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var john = new Person{ID = 1, Name = "John"};
					var garfield = new DomesticCat{ID = 2, Name = "Garfield", Owner = john};
					await (session.SaveAsync(john));
					await (session.SaveAsync(garfield));
					var alice = new Person{ID = 3, Name = "Alice"};
					var bubbles = new Goldfish{ID = 4, Name = "Bubbles", Owner = alice};
					await (session.SaveAsync(alice));
					await (session.SaveAsync(bubbles));
					var pirate = new Person{ID = 5, Name = "Pirate"};
					var parrot = new Parrot{ID = 6, Name = "Parrot", Pirate = pirate};
					await (session.SaveAsync(pirate));
					await (session.SaveAsync(parrot));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task CanGetDomesticCatAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var domesticCat = await (session.GetAsync<DomesticCat>(2));
					Assert.IsNotNull(domesticCat);
					Assert.AreEqual("Garfield", domesticCat.Name);
					Assert.IsNotNull(domesticCat.Owner);
					Assert.AreEqual("John", domesticCat.Owner.Name);
				}
		}

		[Test]
		public async Task CanGetDomesticCatAsCatAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var cat = await (session.GetAsync<Cat>(2));
					Assert.IsNotNull(cat);
					Assert.AreEqual("Garfield", cat.Name);
					var domesticCat = cat as DomesticCat;
					Assert.IsNotNull(domesticCat);
					Assert.IsNotNull(domesticCat.Owner);
					Assert.AreEqual("John", domesticCat.Owner.Name);
				}
		}

		[Test]
		public async Task CanGetGoldfishAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var goldfish = await (session.GetAsync<Goldfish>(4));
					Assert.IsNotNull(goldfish);
					Assert.AreEqual("Bubbles", goldfish.Name);
					Assert.IsNotNull(goldfish.Owner);
					Assert.AreEqual("Alice", goldfish.Owner.Name);
				}
		}

		[Test]
		public async Task CanGetGoldfishAsFishAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var fish = await (session.GetAsync<Fish>(4));
					Assert.IsNotNull(fish);
					Assert.AreEqual("Bubbles", fish.Name);
					var goldfish = fish as Goldfish;
					Assert.IsNotNull(goldfish);
					Assert.IsNotNull(goldfish.Owner);
					Assert.AreEqual("Alice", goldfish.Owner.Name);
				}
		}

		[Test]
		public async Task CanGetParrotAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var parrot = await (session.GetAsync<Parrot>(6));
					Assert.IsNotNull(parrot, "Parrot");
					Assert.AreEqual("Parrot", parrot.Name, "Parrot Name");
					Assert.IsNotNull(parrot.Pirate, "Pirate");
					Assert.AreEqual("Pirate", parrot.Pirate.Name, "Pirate Name");
				}
		}

		[Test]
		public async Task CanGetParrotAsBirdAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var bird = await (session.GetAsync<Bird>(6));
					Assert.IsNotNull(bird, "Bird");
					Assert.AreEqual("Parrot", bird.Name, "Bird Name");
					var parrot = bird as Parrot;
					Assert.IsNotNull(parrot, "Parrot");
					Assert.IsNotNull(parrot.Pirate, "Pirate");
					Assert.AreEqual("Pirate", parrot.Pirate.Name, "Pirate Name");
				}
		}
	}
}
#endif
