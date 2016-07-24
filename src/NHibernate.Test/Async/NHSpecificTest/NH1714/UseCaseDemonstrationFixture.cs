#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Event;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1714
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UseCaseDemonstrationFixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			var listener = new IPreInsertEventListener[this.cfg.EventListeners.PreInsertEventListeners.Length + 1];
			this.cfg.EventListeners.PreInsertEventListeners.CopyTo(listener, 0);
			listener[listener.Length - 1] = new MyCustomEventListener();
			this.cfg.EventListeners.PreInsertEventListeners = listener;
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect as MsSql2005Dialect != null;
		}

		[Test]
		public async Task DbCommandsFromEventListenerShouldBeEnlistedInRunningTransactionAsync()
		{
			using (ISession session = this.OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					var entity = new DomainClass();
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = this.OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from DomainClass"));
					await (session.DeleteAsync("from LogClass"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
