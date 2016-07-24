#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2148
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BugFixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.PersistAsync(new Book{Id = 1, ALotOfText = "a lot of text ..."}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					Assert.That(await (s.CreateSQLQuery("delete from Book").ExecuteUpdateAsync()), Is.EqualTo(1));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanCallLazyPropertyEntityMethodAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1)) as IBook;
				Assert.IsNotNull(book);
				string s1 = "testing1";
				string s2 = book.SomeMethod(s1);
				Assert.AreEqual(s1, s2);
			}
		}
	}
}
#endif
