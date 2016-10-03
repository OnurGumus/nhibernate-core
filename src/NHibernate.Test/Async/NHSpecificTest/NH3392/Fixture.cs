#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3392
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Kid"));
					await (session.DeleteAsync("from FriendOfTheFamily"));
					await (session.DeleteAsync("from Dad"));
					await (session.DeleteAsync("from Mum"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ExpandSubCollectionWithEmbeddedCompositeIDAsync()
		{
			using (ISession s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var jenny = new Mum{Name = "Jenny"};
					await (s.SaveAsync(jenny));
					var benny = new Dad{Name = "Benny"};
					await (s.SaveAsync(benny));
					var lenny = new Dad{Name = "Lenny"};
					await (s.SaveAsync(lenny));
					var jimmy = new Kid{Name = "Jimmy", MumId = jenny.Id, DadId = benny.Id};
					await (s.SaveAsync(jimmy));
					var timmy = new Kid{Name = "Timmy", MumId = jenny.Id, DadId = lenny.Id};
					await (s.SaveAsync(timmy));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
			{
				var result = s.Query<Mum>().Select(x => new
				{
				x, x.Kids
				}

				).ToList();
				Assert.That(result.Count, Is.EqualTo(1));
				Assert.That(result[0].x.Kids, Is.EquivalentTo(result[0].Kids));
			}
		}

		[Test]
		public async Task ExpandSubCollectionWithCompositeIDAsync()
		{
			using (ISession s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var jenny = new Mum{Name = "Jenny"};
					await (s.SaveAsync(jenny));
					var benny = new Dad{Name = "Benny"};
					await (s.SaveAsync(benny));
					var lenny = new Dad{Name = "Lenny"};
					await (s.SaveAsync(lenny));
					var jimmy = new FriendOfTheFamily{Name = "Jimmy", Id = new MumAndDadId{MumId = jenny.Id, DadId = benny.Id}};
					await (s.SaveAsync(jimmy));
					var timmy = new FriendOfTheFamily{Name = "Timmy", Id = new MumAndDadId{MumId = jenny.Id, DadId = lenny.Id}};
					await (s.SaveAsync(timmy));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
			{
				var result = s.Query<Mum>().Select(x => new
				{
				x, x.Friends
				}

				).ToList();
				Assert.That(result.Count, Is.EqualTo(1));
				Assert.That(result[0].x.Friends, Is.EquivalentTo(result[0].Friends));
			}
		}
	}
}
#endif
