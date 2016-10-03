#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Events.PostEvents
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PostUpdateFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Events.PostEvents.SimpleEntity.hbm.xml"};
			}
		}

		[Test]
		public async Task ImplicitFlushAsync()
		{
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]{new AssertOldStatePostListener(eArgs => Assert.That(eArgs.OldState, Is.Not.Null))};
			await (FillDbAsync());
			using (var ls = new LogSpy(typeof (AssertOldStatePostListener)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						IList<SimpleEntity> l = await (s.CreateCriteria<SimpleEntity>().ListAsync<SimpleEntity>());
						l[0].Description = "Modified";
						await (tx.CommitAsync());
					}
				}

				Assert.That(ls.GetWholeLog(), Is.StringContaining(AssertOldStatePostListener.LogMessage));
			}

			await (DbCleanupAsync());
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
		}

		[Test]
		public async Task ExplicitUpdateAsync()
		{
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]{new AssertOldStatePostListener(eArgs => Assert.That(eArgs.OldState, Is.Not.Null))};
			await (FillDbAsync());
			using (var ls = new LogSpy(typeof (AssertOldStatePostListener)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						IList<SimpleEntity> l = await (s.CreateCriteria<SimpleEntity>().ListAsync<SimpleEntity>());
						l[0].Description = "Modified";
						await (s.UpdateAsync(l[0]));
						await (tx.CommitAsync());
					}
				}

				Assert.That(ls.GetWholeLog(), Is.StringContaining(AssertOldStatePostListener.LogMessage));
			}

			await (DbCleanupAsync());
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
		}

		[Test]
		public async Task WithDetachedObjectAsync()
		{
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]{new AssertOldStatePostListener(eArgs => Assert.That(eArgs.OldState, Is.Not.Null))};
			await (FillDbAsync());
			SimpleEntity toModify;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<SimpleEntity> l = await (s.CreateCriteria<SimpleEntity>().ListAsync<SimpleEntity>());
					toModify = l[0];
					await (tx.CommitAsync());
				}
			}

			toModify.Description = "Modified";
			using (var ls = new LogSpy(typeof (AssertOldStatePostListener)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						s.Merge(toModify);
						await (tx.CommitAsync());
					}
				}

				Assert.That(ls.GetWholeLog(), Is.StringContaining(AssertOldStatePostListener.LogMessage));
			}

			await (DbCleanupAsync());
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
		}

		[Test]
		public async Task UpdateDetachedObjectAsync()
		{
			// When the update is used directly as method to reattach a entity the OldState is null
			// that mean that NH should not retrieve info from DB
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]{new AssertOldStatePostListener(eArgs => Assert.That(eArgs.OldState, Is.Null))};
			await (FillDbAsync());
			SimpleEntity toModify;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<SimpleEntity> l = await (s.CreateCriteria<SimpleEntity>().ListAsync<SimpleEntity>());
					toModify = l[0];
					await (tx.CommitAsync());
				}
			}

			toModify.Description = "Modified";
			using (var ls = new LogSpy(typeof (AssertOldStatePostListener)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						await (s.UpdateAsync(toModify));
						await (tx.CommitAsync());
					}
				}

				Assert.That(ls.GetWholeLog(), Is.StringContaining(AssertOldStatePostListener.LogMessage));
			}

			await (DbCleanupAsync());
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
		}

		[Test]
		public async Task UpdateDetachedObjectWithLockAsync()
		{
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[]{new AssertOldStatePostListener(eArgs => Assert.That(eArgs.OldState, Is.Not.Null))};
			await (FillDbAsync());
			SimpleEntity toModify;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<SimpleEntity> l = await (s.CreateCriteria<SimpleEntity>().ListAsync<SimpleEntity>());
					toModify = l[0];
					await (tx.CommitAsync());
				}
			}

			using (var ls = new LogSpy(typeof (AssertOldStatePostListener)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						await (s.LockAsync(toModify, LockMode.None));
						toModify.Description = "Modified";
						await (s.UpdateAsync(toModify));
						await (tx.CommitAsync());
					}
				}

				Assert.That(ls.GetWholeLog(), Is.StringContaining(AssertOldStatePostListener.LogMessage));
			}

			await (DbCleanupAsync());
			((SessionFactoryImpl)sessions).EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
		}

		private async Task DbCleanupAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from SimpleEntity").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}

		private async Task FillDbAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new SimpleEntity{Description = "Something"}));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
