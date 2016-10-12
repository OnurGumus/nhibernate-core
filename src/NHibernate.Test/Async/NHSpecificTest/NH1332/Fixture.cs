#if NET_4_5
using log4net;
using NHibernate.Cfg;
using NHibernate.Event;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1332
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (FixtureAsync));
		public override string BugNumber
		{
			get
			{
				return "NH1332";
			}
		}

		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.UseSecondLevelCache, "false");
			configuration.SetProperty(Environment.UseQueryCache, "false");
			configuration.SetProperty(Environment.CacheProvider, null);
			configuration.SetListener(ListenerType.PostCommitDelete, new PostCommitDelete());
		}

		[Test]
		public async Task BugAsync()
		{
			A a = new A("NH1332");
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (tx.CommitAsync());
				}

			using (LogSpy ls = new LogSpy(log))
			{
				using (ISession s = OpenSession())
					using (ITransaction tx = s.BeginTransaction())
					{
						await (s.DeleteAsync(a));
						await (tx.CommitAsync());
					}

				Assert.AreEqual(1, ls.Appender.GetEvents().Length);
				string logs = ls.Appender.GetEvents()[0].RenderedMessage;
				Assert.Greater(logs.IndexOf("PostCommitDelete fired."), -1);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PostCommitDelete : IPostDeleteEventListener
		{
			public void OnPostDelete(PostDeleteEvent @event)
			{
				log.Debug("PostCommitDelete fired.");
			}

			public Task OnPostDeleteAsync(PostDeleteEvent @event)
			{
				try
				{
					log.Debug("PostCommitDelete fired.");
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PostCommitDelete : IPostDeleteEventListener
		{
			public Task OnPostDeleteAsync(PostDeleteEvent @event)
			{
				try
				{
					log.Debug("PostCommitDelete fired.");
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}
	}
}
#endif
