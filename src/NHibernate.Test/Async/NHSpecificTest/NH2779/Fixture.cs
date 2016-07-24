#if NET_4_5
using log4net;
using log4net.Core;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2779
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Order order = new Order{OrderId = "Order-1", InternalOrderId = 1};
					await (session.SaveAsync(order));
					await (tx.CommitAsync());
				}

			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
					using (new LogSpy(LogManager.GetLogger("NHibernate"), Level.All)) //  <-- Logging must be set DEBUG to reproduce bug
					{
						Order order = await (session.GetAsync<Order>("Order-1"));
						Assert.IsNotNull(order);
						// Cleanup
						await (session.DeleteAsync(order));
						await (tx.CommitAsync());
					}
		}
	}
}
#endif
