#if NET_4_5
using System;
using System.Linq.Expressions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2733
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseExpressionForWhereAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Item, Item> query = session.QueryOver(() => Item);
				var start = DateTime.UtcNow;
				query.Where(GetExpression(start));
				await (query.ListAsync());
			}
		}
	}
}
#endif
