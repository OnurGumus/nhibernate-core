#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2806
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
		public async Task SelectPregnantStatusOfTypeHqlAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var list = await (session.CreateQuery("select a.Pregnant from Animal a where a.class in ('MAMMAL')").ListAsync<bool>());
					var count = list.Count();
					Assert.AreEqual(0, count);
				}
		}

		[Test]
		public async Task SelectAllAnimalsShouldPerformJoinsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					using (var spy = new SqlLogSpy())
					{
						var list = await (session.CreateQuery("from Animal").ListAsync<Animal>());
						var count = list.Count();
						Assert.AreEqual(3, count);
						Assert.Greater(1, spy.GetWholeLog().Split(new[]{"inner join"}, StringSplitOptions.None).Count());
					}
				}
		}
	}
}
#endif
