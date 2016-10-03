#if NET_4_5
using System.Collections;
using System.Transactions;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SystemTransactions
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TransactionFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"WZ.hbm.xml"};
			}
		}

		[Test]
		public async Task CanUseSystemTransactionsToCommitAsync()
		{
			object identifier;
			using (ISession session = sessions.OpenSession())
				using (TransactionScope tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					W s = new W();
					await (session.SaveAsync(s));
					identifier = s.Id;
					tx.Complete();
				}

			using (ISession session = sessions.OpenSession())
				using (TransactionScope tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					W w = await (session.GetAsync<W>(identifier));
					Assert.IsNotNull(w);
					await (session.DeleteAsync(w));
					tx.Complete();
				}
		}
	}
}
#endif
