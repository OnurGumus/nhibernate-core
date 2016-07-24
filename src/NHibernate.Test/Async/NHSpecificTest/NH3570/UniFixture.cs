#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3570
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UniFixtureAsync : BugTestCaseAsync
	{
		private Guid id;
		[Test]
		public async Task ShouldNotSaveRemoveChildAsync()
		{
			Assert.ThrowsAsync<Exception>(async () =>
			{
				var parent = new UniParent();
				parent.Children.Add(new UniChild());
				using (var s = OpenSession())
				{
					using (var tx = s.BeginTransaction())
					{
						id = (Guid)await (s.SaveAsync(parent));
						parent.Children.Clear();
						parent.Children.Add(new UniChild());
						await (tx.CommitAsync());
					}
				}

				using (var s = OpenSession())
				{
					using (s.BeginTransaction())
					{
						Assert.That((await (s.GetAsync<UniParent>(id))).Children.Count, Is.EqualTo(1));
						Assert.That((await (s.CreateCriteria<UniChild>().ListAsync())).Count, Is.EqualTo(1));
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
					await (s.CreateQuery("delete from UniChild").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from UniParent").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
