#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3182
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			var mother = new Lizard{BodyWeight = 48, Description = "Mother", Children = new List<Animal>()};
			var father = new Lizard{BodyWeight = 48, Description = "Father", Children = new List<Animal>()};
			var child = new Lizard{Mother = mother, Father = father, BodyWeight = 48, Description = "Child", };
			mother.Children.Add(child);
			father.Children.Add(child);
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(mother));
					await (session.SaveAsync(father));
					await (session.SaveAsync(child));
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
		public async Task SelectManyPregnantStatusCast1Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await ((session.Query<Animal>().SelectMany(o => o.Children).Where(o => o is Mammal).Select(o => ((Mammal)o).Pregnant)).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectManyPregnantStatusCast2Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await ((session.Query<Animal>().SelectMany(o => o.Children).Where(o => o is Mammal).Select(o => ((Mammal)o).Pregnant)).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectManyPregnantStatusOfType1Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await (session.Query<Animal>().SelectMany(o => o.Children, (animal, animal1) => animal1).OfType<Mammal>().Select(o => o.Pregnant).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectManyPregnantStatusOfType2Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await (session.Query<Animal>().SelectMany(o => o.Children, (animal, animal1) => animal1).OfType<Mammal>().Select(o => o.Pregnant).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectPregnantStatusCastAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await ((session.Query<Animal>().Where(o => o is Mammal).Select(o => ((Mammal)o).Pregnant)).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectPregnantStatusOfTypeAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await (session.Query<Animal>().OfType<Mammal>().Select(o => o.Pregnant).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectPregnantStatusAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await (session.Query<Mammal>().Select(o => o.Pregnant).ToListAsync());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}
	}
}
#endif
