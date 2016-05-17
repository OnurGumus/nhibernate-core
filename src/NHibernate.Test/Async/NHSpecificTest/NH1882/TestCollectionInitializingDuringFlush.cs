#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1882
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestCollectionInitializingDuringFlush : TestCaseMappingByCode
	{
		[Test]
		public async Task TestInitializationDuringFlushAsync()
		{
			Assert.False(listener.Executed);
			Assert.False(listener.FoundAny);
			ISession s = OpenSession();
			s.BeginTransaction();
			var publisher = new Publisher("acme");
			var author = new Author("john");
			author.Publisher = publisher;
			publisher.Authors.Add(author);
			author.Books.Add(new Book("Reflections on a Wimpy Kid", author));
			await (s.SaveAsync(author));
			await (s.Transaction.CommitAsync());
			s.Clear();
			s = OpenSession();
			s.BeginTransaction();
			publisher = await (s.GetAsync<Publisher>(publisher.Id));
			publisher.Name = "random nally";
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Clear();
			s = OpenSession();
			s.BeginTransaction();
			await (s.DeleteAsync(author));
			await (s.Transaction.CommitAsync());
			s.Clear();
			s.Close();
			Assert.That(listener.Executed, Is.True);
			Assert.That(listener.FoundAny, Is.True);
		}
	}
}
#endif
