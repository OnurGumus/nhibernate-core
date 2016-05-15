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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UseCaseDemonstrationFixture : BugTestCase
	{
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

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyCustomEventListener : IPreInsertEventListener
	{
		public async Task<bool> OnPreInsertAsync(PreInsertEvent e)
		{
			if (e.Entity is DomainClass == false)
				return false;
			// this will join into the parent's transaction
			using (var session = e.Session.GetSession(EntityMode.Poco))
			{
				//should insert log record here
				await (session.SaveAsync(new LogClass()));
				await (session.FlushAsync());
			}

			return false;
		}
	}
}
#endif
