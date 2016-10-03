#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1813
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.BatchSize, "0");
		}

		[Test]
		public async Task ContainSQLInInsertAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new EntityWithUnique{Id = 1, Description = "algo"}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new EntityWithUnique{Id = 2, Description = "algo"}));
					var exception = Assert.ThrowsAsync<GenericADOException>(async () => await t.CommitAsync());
					Assert.That(exception.Message, Is.StringContaining("INSERT"), "should contain SQL");
					Assert.That(exception.Message, Is.StringContaining("#2"), "should contain id");
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from EntityWithUnique").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ContainSQLInUpdateAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new EntityWithUnique{Id = 1, Description = "algo"}));
					await (s.SaveAsync(new EntityWithUnique{Id = 2, Description = "mas"}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var e = await (s.GetAsync<EntityWithUnique>(2));
					e.Description = "algo";
					var exception = Assert.ThrowsAsync<GenericADOException>(async () => await t.CommitAsync());
					Assert.That(exception.Message, Is.StringContaining("UPDATE"), "should contain SQL");
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from EntityWithUnique").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
