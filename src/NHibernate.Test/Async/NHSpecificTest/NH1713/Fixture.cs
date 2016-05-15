#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1713
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
