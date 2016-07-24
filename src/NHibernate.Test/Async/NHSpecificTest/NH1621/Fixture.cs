#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1621
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		ISession session;
		public override string BugNumber
		{
			get
			{
				return "NH1621";
			}
		}

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
				var nums = await (session.CreateQuery("from Nums b where b.Sum > 4").ListAsync<Nums>());
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
