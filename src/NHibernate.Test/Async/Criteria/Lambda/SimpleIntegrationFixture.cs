#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria.Lambda
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleIntegrationFixture : TestCase
	{
		[Test]
		public async Task TestQueryOverAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					Person personAlias = null;
					var actual = await (s.QueryOver<Person>(() => personAlias).Where(() => personAlias.Name == "test person 2").And(() => personAlias.Age == 30).ListAsync());
					Assert.That(actual.Count, Is.EqualTo(1));
				}
		}

		[Test]
		public async Task TestQueryOverAliasAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					Person personAlias = null;
					var actual = await (s.QueryOver<Person>(() => personAlias).Where(() => personAlias.Name == "test person 2").And(() => personAlias.Age == 30).ListAsync());
					Assert.That(actual.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
