#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SelfJoinTestFixtureAsync : BugTestCaseAsync
	{
		[Test]
		public void SelfJoinsWorkIfNothingJoins()
		{
			using (var session = OpenSession())
			{
				var result = (
					from bo in session.Query<MyBO>()where bo.LeftSon.LeftSon.K2 == 2 || bo.RightSon.RightSon.K2 == 3
					select bo).ToList().Select(bo => bo.Name);
				Assert.AreEqual(0, result.Count());
			}
		}

		[Test]
		public void SelfJoinsWorkIfOneSideJoins()
		{
			using (var session = OpenSession())
			{
				var result = (
					from bo in session.Query<MyBO>()where bo.LeftSon.LeftSon.K2 == 1 || bo.RightSon.RightSon.K2 == 2
					select bo).ToList().Select(bo => bo.Name);
				Assert.That(() => result.ToList(), Is.EquivalentTo(new[]{"1", "2"}));
			}
		}

		[Test]
		public void SelfJoinsWorkIfBothSidesJoin()
		{
			using (var session = OpenSession())
			{
				var result = (
					from bo in session.Query<MyBO>()where bo.LeftSon.LeftSon.K2 == 3 || bo.RightSon.RightSon.K2 == 4
					select bo).ToList().Select(bo => bo.Name);
				Assert.That(() => result.ToList(), Is.EquivalentTo(new[]{"3"}));
			}
		}

		[Test]
		public void SelfJoinsWorkForNullCompares()
		{
			using (var session = OpenSession())
			{
				var result = (
					from bo in session.Query<MyBO>()where bo.LeftSon.LeftSon.K1 == null || bo.RightSon.RightSon.K1 == null
					select bo).ToList().Select(bo => bo.Name);
				Assert.That(!result.Contains("1"));
				Assert.That(result.Count(), Is.EqualTo(18));
			}
		}

		[Test]
		public void SelfJoinsWorkForNullCompares2()
		{
			using (var session = OpenSession())
			{
				var result = (
					from bo in session.Query<MyBO>()where bo.LeftSon.K1 == null || bo.RightSon.K1 == null
					select bo).ToList().Select(bo => bo.Name);
				Assert.That(!result.Contains("1"));
				Assert.That(result.Count(), Is.EqualTo(18));
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					{
						var bLeftLeft = new MyBO{Id = 111, Name = "1LL", K2 = 1, K1 = 1};
						var bLeftRight = new MyBO{Id = 112, Name = "1LR", K2 = 1};
						var bLeft = new MyBO{Id = 11, Name = "1L", LeftSon = bLeftLeft, RightSon = bLeftRight, K1 = 1};
						var bRightRight = new MyBO{Id = 122, Name = "1RR", K2 = 1, K1 = 1};
						var bRight = new MyBO{Id = 12, Name = "1R", RightSon = bRightRight, K1 = 1};
						var bRoot = new MyBO{Id = 1, Name = "1", LeftSon = bLeft, RightSon = bRight};
						await (session.SaveAsync(bLeftLeft));
						await (session.SaveAsync(bLeftRight));
						await (session.SaveAsync(bLeft));
						await (session.SaveAsync(bRightRight));
						await (session.SaveAsync(bRight));
						await (session.SaveAsync(bRoot));
					}

					{
						var bLeftRight = new MyBO{Id = 212, Name = "2LR", K2 = 2};
						var bLeft = new MyBO{Id = 21, Name = "2L", RightSon = bLeftRight};
						var bRightRight = new MyBO{Id = 222, Name = "2RR", K2 = 2};
						var bRight = new MyBO{Id = 22, Name = "2R", RightSon = bRightRight};
						var bRoot = new MyBO{Id = 2, Name = "2", LeftSon = bLeft, RightSon = bRight};
						await (session.SaveAsync(bLeftRight));
						await (session.SaveAsync(bLeft));
						await (session.SaveAsync(bRightRight));
						await (session.SaveAsync(bRight));
						await (session.SaveAsync(bRoot));
					}

					{
						var bLeftLeft = new MyBO{Id = 311, Name = "3LL", K2 = 3};
						var bLeftRight = new MyBO{Id = 312, Name = "3LR", K2 = 3};
						var bLeft = new MyBO{Id = 31, Name = "3L", LeftSon = bLeftLeft, RightSon = bLeftRight};
						var bRight = new MyBO{Id = 32, Name = "3R"};
						var bRoot = new MyBO{Id = 3, Name = "3", LeftSon = bLeft, RightSon = bRight};
						await (session.SaveAsync(bLeftLeft));
						await (session.SaveAsync(bLeftRight));
						await (session.SaveAsync(bLeft));
						await (session.SaveAsync(bRight));
						await (session.SaveAsync(bRoot));
					}

					{
						var bLeft = new MyBO{Id = 41, Name = "4L"};
						var bRight = new MyBO{Id = 42, Name = "4R"};
						var bRoot = new MyBO{Id = 4, Name = "4", LeftSon = bLeft, RightSon = bRight};
						await (session.SaveAsync(bLeft));
						await (session.SaveAsync(bRight));
						await (session.SaveAsync(bRoot));
					}

					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
