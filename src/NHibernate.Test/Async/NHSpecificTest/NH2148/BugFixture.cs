#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2148
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BugFixture : BugTestCase
	{
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
