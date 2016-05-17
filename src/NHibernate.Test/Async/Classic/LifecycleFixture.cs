#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Classic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LifecycleFixture : TestCase
	{
		[Test]
		public async Task SaveAsync()
		{
			sessions.Statistics.Clear();
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(new EntityWithLifecycle()));
				await (s.FlushAsync());
			}

			Assert.That(sessions.Statistics.EntityInsertCount, Is.EqualTo(0));
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task UpdateAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}

			// update detached
			sessions.Statistics.Clear();
			v.Heigth = 0;
			using (ISession s = OpenSession())
			{
				await (s.UpdateAsync(v));
				await (s.FlushAsync());
			}

			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(0));
			// cleanup
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdate();
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task MergeAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}

			v.Heigth = 0;
			sessions.Statistics.Clear();
			using (ISession s = OpenSession())
			{
				s.Merge(v);
				await (s.FlushAsync());
			}

			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(0));
			var v1 = new EntityWithLifecycle("Shinobi", 0, 10);
			using (ISession s = OpenSession())
			{
				s.Merge(v1);
				await (s.FlushAsync());
			}

			Assert.That(sessions.Statistics.EntityInsertCount, Is.EqualTo(0));
			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(0));
			// cleanup
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdate();
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task DeleteAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
				sessions.Statistics.Clear();
				v.Heigth = 0;
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
				Assert.That(sessions.Statistics.EntityDeleteCount, Is.EqualTo(0));
			}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdate();
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
