#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ListIndex
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleOneToManyTest : TestCase
	{
		[Test]
		public async Task ShouldIncludeTheListIdxInsertingAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var galery = new Galery();
					galery.Images.Add(new Image{Path = "image01.jpg"});
					await (s.PersistAsync(galery));
					Assert.DoesNotThrow(async () => await tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from Image").ExecuteUpdate();
					s.CreateQuery("delete from Galery").ExecuteUpdate();
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
