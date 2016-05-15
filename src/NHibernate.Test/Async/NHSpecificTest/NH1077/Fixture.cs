#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1077
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task LokingAsync()
		{
			object savedId;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					A a = new A("hunabKu");
					savedId = await (s.SaveAsync(a));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					A a = await (s.GetAsync<A>(savedId));
					using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
					{
						await (s.LockAsync(a, LockMode.Upgrade));
						string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
						Assert.Less(0, sql.IndexOf("with (updlock"));
					}

					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from A"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
