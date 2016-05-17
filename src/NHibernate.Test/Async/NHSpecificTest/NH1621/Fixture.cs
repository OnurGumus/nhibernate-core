#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1621
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task QueryUsingReadonlyPropertyAsync()
		{
			using (session = OpenSession())
			{
				Nums nums1 = new Nums{ID = 1, NumA = 1, NumB = 2};
				await (session.SaveAsync(nums1));
				Nums nums2 = new Nums{ID = 2, NumA = 2, NumB = 2};
				await (session.SaveAsync(nums2));
				Nums nums3 = new Nums{ID = 3, NumA = 5, NumB = 2};
				await (session.SaveAsync(nums3));
				await (session.FlushAsync());
				session.Clear();
				var nums = session.CreateQuery("from Nums b where b.Sum > 4").List<Nums>();
				Assert.That(nums.Count, Is.EqualTo(1));
				Assert.That(nums[0].Sum, Is.EqualTo(7));
				await (session.DeleteAsync("from Nums"));
				await (session.FlushAsync());
				session.Close();
			}
		}
	}
}
#endif
