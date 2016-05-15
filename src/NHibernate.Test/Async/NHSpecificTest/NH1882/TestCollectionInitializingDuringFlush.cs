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

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InitializingPreUpdateEventListener : IPreUpdateEventListener
		{
			public async Task<bool> OnPreUpdateAsync(PreUpdateEvent @event)
			{
				Executed = true;
				Object[] oldValues = @event.OldState;
				String[] properties = @event.Persister.PropertyNames;
				// Iterate through all fields of the updated object
				for (int i = 0; i < properties.Length; i++)
				{
					if (oldValues != null && oldValues[i] != null)
					{
						if (!NHibernateUtil.IsInitialized(oldValues[i]))
						{
							// force any proxies and/or collections to initialize to illustrate HHH-2763
							FoundAny = true;
							await (NHibernateUtil.InitializeAsync(oldValues[i]));
						}
					}
				}

				return true;
			}
		}
	}
}
#endif
