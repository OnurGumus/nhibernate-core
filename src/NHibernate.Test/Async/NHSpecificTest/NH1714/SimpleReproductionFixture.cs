#if NET_4_5
using System;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1714
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleReproductionFixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect as MsSql2005Dialect != null;
		}

		[Test]
		public async Task DbCommandsFromEventListenerShouldBeEnlistedInRunningTransactionAsync()
		{
			using (ISession session = OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					var entity = new DomainClass();
					await (session.SaveAsync(entity));
					using (var otherSession = session.GetSession(EntityMode.Poco))
					{
						await (otherSession.SaveAsync(new DomainClass()));
						await (otherSession.FlushAsync());
					}

					await (tx.CommitAsync());
				}
			}

			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from DomainClass"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
