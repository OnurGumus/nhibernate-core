#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1756
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SaveTransient_Then_Update_OkAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var book = new BookNotGenerated{Name = "test book", Pages = new List<Page>(), };
					await (session.SaveAsync(book));
					book.Name = "modified test book";
					await (transaction.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					session.CreateQuery("delete from BookNotGenerated").ExecuteUpdate();
					await (transaction.CommitAsync());
				}
			}
		}

		[Test]
		[Description("Work with AutoFlush on commit")]
		public async Task SaveTransient_Then_UpdateAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var book = new Book{Name = "test book", Pages = new List<Page>(), };
					await (session.SaveAsync(book));
					book.Name = "modified test book";
					await (transaction.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					session.CreateQuery("delete from Book").ExecuteUpdate();
					await (transaction.CommitAsync());
				}
			}
		}

		[Test]
		public async Task SaveTransient_Then_Update_BugAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var book = new Book{Name = "test book", Pages = new List<Page>(), };
					await (session.SaveAsync(book));
					book.Name = "modified test book";
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					session.CreateQuery("delete from Book").ExecuteUpdate();
					await (transaction.CommitAsync());
				}
			}
		}
	}
}
#endif
