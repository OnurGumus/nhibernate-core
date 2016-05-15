#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1981
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanGroupWithParameterAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					const string queryString = @"select (Longitude / :divisor)
					  from Article
					  group by (Longitude / :divisor)
					  order by 1";
					var quotients = await (s.CreateQuery(queryString).SetDouble("divisor", 30).ListAsync<double>());
					Assert.That(quotients.Count, Is.EqualTo(2));
					Assert.That(quotients[0], Is.EqualTo(3));
					Assert.That(quotients[1], Is.EqualTo(4));
				}
		}
	}
}
#endif
