#if NET_4_5
using System.Collections;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.LazyProperty
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyPropertyFixtureAsync : TestCaseAsync
	{
		private string log;
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"LazyProperty.Mappings.hbm.xml"};
			}
		}

		protected override void BuildSessionFactory()
		{
			using (var logSpy = new LogSpy(typeof (EntityMetamodel)))
			{
				base.BuildSessionFactory();
				log = logSpy.GetWholeLog();
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.PersistAsync(new Book{Name = "some name", Id = 1, ALotOfText = "a lot of text ..."}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					Assert.That(await (s.CreateSQLQuery("delete from Book").ExecuteUpdateAsync()), Is.EqualTo(1));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task PropertyLoadedNotInitializedAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.LoadAsync<Book>(1));
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Id")), Is.False);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Name")), Is.False);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.False);
				await (NHibernateUtil.InitializeAsync(book));
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Id")), Is.True);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Name")), Is.True);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.False);
			}
		}

		[Test]
		public async Task PropertyLoadedNotInitializedWhenUsingGetAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Id")), Is.True);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "Name")), Is.True);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.False);
			}
		}

		[Test]
		public async Task CanGetValueForLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(book.ALotOfText, Is.EqualTo("a lot of text ..."));
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.True);
			}
		}

		[Test]
		public async Task CanGetValueForNonLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				Assert.That(book.Name, Is.EqualTo("some name"));
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.False);
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
					Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.False);
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
