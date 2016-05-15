#if NET_4_5
using System.Text;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1635
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private async Task CreateTestContextAsync()
		{
			var t1 = new ForumThread{Id = 1, Name = "Thread 1"};
			var t2 = new ForumThread{Id = 2, Name = "Thread 2"};
			var m1 = new ForumMessage{Id = 1, Name = "Thread 1: Message 1", ForumThread = t1};
			var m2 = new ForumMessage{Id = 2, Name = "Thread 1: Message 2", ForumThread = t1};
			var m3 = new ForumMessage{Id = 3, Name = "Thread 2: Message 1", ForumThread = t2};
			t1.Messages.Add(m1);
			t1.Messages.Add(m2);
			t2.Messages.Add(m3);
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(t1));
					await (session.SaveAsync(t2));
					await (transaction.CommitAsync());
				}
			}
		}

		private async Task CleanUpAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from ForumMessage"));
				await (session.DeleteAsync("from ForumThread"));
				await (session.FlushAsync());
			}
		}

		protected override async Task CreateSchemaAsync()
		{
			var script = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => script.Append(sl), true));
			Assert.That(script.ToString(), Is.Not.StringContaining("LatestMessage"));
		}

		[Test]
		public async Task TestAsync()
		{
			await (CreateTestContextAsync());
			using (ISession session = OpenSession())
			{
				var thread = await (session.GetAsync<ForumThread>(1));
				Assert.IsNotNull(thread.LatestMessage);
				Assert.IsTrue(thread.LatestMessage.Id == 2);
				await (session.FlushAsync());
			}

			await (CleanUpAsync());
		}
	}
}
#endif
