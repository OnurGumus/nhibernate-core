#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1713
{
	[TestFixture, Ignore("Should be fixed in some way.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		/* NOTE
		 * This test should be fixed in some way at least to support Money.
		 * So far it is only a demostration that using 
		 * <property name="prepare_sql">false</property>
		 * we should do some additional work for INSERT+UPDATE
		 */
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.PrepareSql, "true");
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task Can_Save_Money_ColumnAsync()
		{
			Assert.That(PropertiesHelper.GetBoolean(Environment.PrepareSql, cfg.Properties, false));
			var item = new A{Amount = 2600};
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(item));
					await (tx.CommitAsync());
				}
			}

			Assert.IsTrue(item.Id > 0);
			// cleanup
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from A"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task Can_Update_Money_ColumnAsync()
		{
			Assert.That(PropertiesHelper.GetBoolean(Environment.PrepareSql, cfg.Properties, false));
			object savedId;
			var item = new A{Amount = (decimal ? )2600.55};
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					savedId = await (s.SaveAsync(item));
					await (tx.CommitAsync());
				}
			}

			Assert.That(item.Id, Is.GreaterThan(0));
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var item2 = await (s.LoadAsync<A>(savedId));
					item2.Amount = item2.Amount - 1.5m;
					await (s.SaveOrUpdateAsync(item2));
					await (tx.CommitAsync());
				}
			}

			// cleanup
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from A"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
