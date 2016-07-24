#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1275
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1275";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !string.IsNullOrEmpty(dialect.ForUpdateString);
		}

		[Test]
		public async Task RetrievingAsync()
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
					using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
					{
						await (s.GetAsync<A>(savedId, LockMode.Upgrade));
						string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
						Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
					}

					using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
					{
						await (s.CreateQuery("from A a where a.Id= :pid").SetLockMode("a", LockMode.Upgrade).SetParameter("pid", savedId).UniqueResultAsync<A>());
						string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
						Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
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
						Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
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
