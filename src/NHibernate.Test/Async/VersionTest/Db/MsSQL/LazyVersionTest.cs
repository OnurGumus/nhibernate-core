#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.VersionTest.Db.MsSQL
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyVersionTest : TestCase
	{
		[Test(Description = "NH-3589")]
		public async Task CanUseVersionOnEntityWithLazyPropertyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.SaveAsync(new ProductWithVersionAndLazyProperty{Id = 1, Summary = "Testing, 1, 2, 3"}));
					await (session.FlushAsync());
					session.Clear();
					var p = await (session.GetAsync<ProductWithVersionAndLazyProperty>(1));
					p.Summary += ", 4!";
					await (session.FlushAsync());
				}
		}
	}
}
#endif
