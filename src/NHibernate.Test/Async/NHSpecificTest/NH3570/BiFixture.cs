#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3570
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BiFixtureAsync : BugTestCaseAsync
	{
		private Guid id;
		[Test]
		public async Task ShouldNotSaveRemoveChildAsync()
		{
			Assert.ThrowsAsync<AssertionException>(async () =>
			{
				var parent = new BiParent();
				parent.AddChild(new BiChild());
				using (var s = OpenSession())
				{
					using (var tx = s.BeginTransaction())
					{
						id = (Guid)await (s.SaveAsync(parent));
						parent.Children.Clear();
						parent.AddChild(new BiChild());
						await (tx.CommitAsync());
					}
				}

				using (var s = OpenSession())
				{
					using (s.BeginTransaction())
					{
						Assert.That((await (s.GetAsync<BiParent>(id))).Children.Count, Is.EqualTo(1));
						Assert.That((await (s.CreateCriteria<BiChild>().ListAsync())).Count, Is.EqualTo(1));
					}
				}
			}

			, KnownBug.Issue("NH-3570"));
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from BiChild").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from BiParent").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
