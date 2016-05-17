#if NET_4_5
using System.Data.Common;
using System.Collections;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SecondLevelCacheTests
{
	using Criterion;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SecondLevelCacheTest : TestCase
	{
		[Test]
		public async Task DeleteItemFromCollectionThatIsInTheSecondLevelCacheAsync()
		{
			using (ISession session = OpenSession())
			{
				Item item = (Item)session.Load(typeof (Item), 1);
				Assert.IsTrue(item.Children.Count == 4); // just force it into the second level cache here
			}

			int childId = -1;
			using (ISession session = OpenSession())
			{
				Item item = (Item)session.Load(typeof (Item), 1);
				Item child = (Item)item.Children[0];
				childId = child.Id;
				await (session.DeleteAsync(child));
				item.Children.Remove(child);
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				Item item = (Item)session.Load(typeof (Item), 1);
				Assert.AreEqual(3, item.Children.Count);
				foreach (Item child in item.Children)
				{
					NHibernateUtil.Initialize(child);
					Assert.IsFalse(child.Id == childId);
				}
			}
		}

		[Test]
		public async Task InsertItemToCollectionOnTheSecondLevelCacheAsync()
		{
			using (ISession session = OpenSession())
			{
				Item item = (Item)session.Load(typeof (Item), 1);
				Item child = new Item();
				child.Id = 6;
				item.Children.Add(child);
				await (session.SaveAsync(child));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				Item item = (Item)session.Load(typeof (Item), 1);
				int count = item.Children.Count;
				Assert.AreEqual(5, count);
			}
		}
	}
}
#endif
