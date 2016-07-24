#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BulkUpdateWithCustomCompositeTypeAsync : BugTestCaseAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class Scenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public Scenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (ISession s = factory.OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						var e = new Entity{Data = new CompositeData{DataA = new DateTime(2010, 1, 1), DataB = new DateTime(2010, 2, 2)}};
						s.Save(e);
						t.Commit();
					}
			}

			public void Dispose()
			{
				using (ISession s = factory.OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						s.CreateQuery("delete from Entity").ExecuteUpdate();
						t.Commit();
					}
			}
		}

		[Test]
		public async Task ShouldAllowBulkupdateWithCompositeUserTypeAsync()
		{
			using (new Scenario(Sfi))
			{
				string queryString = @"update Entity m set m.Data.DataA = :dataA, m.Data.DataB = :dataB";
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						var query = s.CreateQuery(queryString).SetDateTime("dataA", new DateTime(2010, 3, 3)).SetDateTime("dataB", new DateTime(2010, 4, 4));
						Assert.That(async () => await (query.ExecuteUpdateAsync()), Throws.Nothing);
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
