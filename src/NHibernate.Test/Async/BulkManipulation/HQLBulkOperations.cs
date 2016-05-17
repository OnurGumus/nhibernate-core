#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.BulkManipulation
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HqlBulkOperations : BaseFixture
	{
		[Test]
		public async Task SimpleDeleteAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new SimpleClass{Description = "simple1"}));
					await (s.SaveAsync(new SimpleClass{Description = "simple2"}));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					Assert.That(s.CreateQuery("delete from SimpleClass where Description = 'simple2'").ExecuteUpdate(), Is.EqualTo(1));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					Assert.That(s.CreateQuery("delete from SimpleClass").ExecuteUpdate(), Is.EqualTo(1));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
