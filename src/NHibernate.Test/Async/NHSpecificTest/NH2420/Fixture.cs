#if NET_4_5
using System;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Transactions;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2420
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH2420";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return (dialect is MsSql2005Dialect);
		}

		[Test]
		public async Task ShouldBeAbleToReleaseSuppliedConnectionAfterDistributedTransactionAsync()
		{
			string connectionString = cfg.GetProperty("connection.connection_string");
			ISession s;
			using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				// Enlisting DummyEnlistment as a durable resource manager will start
				// a DTC transaction
				System.Transactions.Transaction.Current.EnlistDurable(DummyEnlistment.Id, new DummyEnlistment(), EnlistmentOptions.None);
				DbConnection connection;
				if (sessions.ConnectionProvider.Driver.GetType() == typeof (OdbcDriver))
					connection = new OdbcConnection(connectionString);
				else
					connection = new SqlConnection(connectionString);
				using (connection)
				{
					await (connection.OpenAsync());
					using (s = Sfi.OpenSession(connection))
					{
						await (s.SaveAsync(new MyTable{String = "hello!"}));
					}

					connection.Close();
				}

				ts.Complete();
			}

			// Prior to the patch, an InvalidOperationException exception would occur in the
			// TransactionCompleted delegate at this point with the message, "Disconnect cannot
			// be called while a transaction is in progress". Although the exception can be
			// seen reported in the IDE, NUnit fails to see it. The TransactionCompleted event
			// fires *after* the transaction is committed and so it doesn't affect the success
			// of the transaction.
			Assert.That(s.IsConnected, Is.False);
			Assert.That(((ISessionImplementor)s).ConnectionManager.IsConnected, Is.False);
			Assert.That(((ISessionImplementor)s).IsClosed, Is.True);
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from MyTable"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
