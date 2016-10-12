#if NET_4_5
using System.Linq;
using System.Text;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionQueringTestAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task WhenQueryThroughStatelessSessionThenDoesNotThrowsAsync()
		{
			using (var statelessSession = Sfi.OpenStatelessSession())
			{
				var query = statelessSession.Query<Customer>();
				Assert.That(async () => await (query.ToListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
