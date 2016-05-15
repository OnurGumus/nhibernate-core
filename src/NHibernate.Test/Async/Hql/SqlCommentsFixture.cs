#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlCommentsFixture : TestCase
	{
		[Test]
		public async Task CommentsInQueryAsync()
		{
			using (ISession s = OpenSession())
			{
				using (var sl = new SqlLogSpy())
				{
					await (s.CreateQuery("from Animal").SetComment("This is my query").ListAsync());
					string sql = sl.Appender.GetEvents()[0].RenderedMessage;
					Assert.That(sql.IndexOf("This is my query"), Is.GreaterThan(0));
				}
			}
		}
	}
}
#endif
