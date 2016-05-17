#if NET_4_5
using System.Collections;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.LazyProperty
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyPropertyFixture : TestCase
	{
		[Test]
		public async Task PropertyLoadedNotInitializedWhenUsingGetAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Id"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Name"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
			}
		}

		[Test]
		public async Task CanGetValueForLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(book.ALotOfText, Is.EqualTo("a lot of text ..."));
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
			}
		}

		[Test]
		public async Task CanGetValueForNonLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(book.Name, Is.EqualTo("some name"));
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
			}
		}

		[Test]
		public async Task CanLoadAndSaveObjectInDifferentSessionsAsync()
		{
			Book book;
			using (ISession s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
			}

			using (ISession s = OpenSession())
			{
				s.Merge(book);
			}
		}

		[Test]
		public async Task CanUpdateNonLazyWithoutLoadingLazyPropertyAsync()
		{
			Book book;
			using (ISession s = OpenSession())
				using (var trans = s.BeginTransaction())
				{
					book = await (s.GetAsync<Book>(1));
					book.Name += "updated";
					Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
					await (trans.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
				Assert.That(book.Name, Is.EqualTo("some nameupdated"));
			}
		}
	}
}
#endif
