#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Query
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SelfReferencingCollectionLoadTest : TestCase
	{
		[Test]
		public async Task LoadCollectionAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Item(1, 2)));
					await (session.SaveAsync(new Item(2, 3)));
					await (session.SaveAsync(new Item(3, 1)));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var item1 = (Item)session.Get(typeof (Item), 1);
					Assert.AreEqual(2, item1.AlternativeItems.Count);
					await (session.DeleteAsync("from Item"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
