#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1192
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BitwiseAndWorksCorrectlyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateQuery("from ObjectA o where (o.FontType & 1) > 0");
					var result = await (query.ListAsync());
					Assert.That(result, Has.Count.EqualTo(1));
					query = session.CreateQuery("from ObjectA o where (o.FontType & 2) > 0");
					result = await (query.ListAsync());
					Assert.That(result, Has.Count.EqualTo(2));
					query = session.CreateQuery("from ObjectA o where (o.FontType & 4) > 0");
					result = await (query.ListAsync());
					Assert.That(result, Has.Count.EqualTo(1));
				}
		}

		[Test]
		public async Task BitwiseOrWorksCorrectlyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateQuery("from ObjectA o where (o.FontType | 2)  = (1|2) ");
					var result = await (query.ListAsync());
					Assert.That(result, Has.Count.EqualTo(1));
				}
		}
	}
}
#endif
