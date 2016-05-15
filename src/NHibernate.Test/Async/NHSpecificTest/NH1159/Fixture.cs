#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1159
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task DoesNotFlushWithCriteriaWithCommitAsync()
		{
			using (ISession session = OpenSession(new HibernateInterceptor()))
				using (ITransaction tran = session.BeginTransaction())
				{
					session.FlushMode = FlushMode.Commit;
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(0));
					Contact contact = await (session.GetAsync<Contact>((Int64)1));
					contact.PreferredName = "Updated preferred name";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					contact.Forename = "Updated forename";
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					ICriteria query = session.CreateCriteria(typeof (ContactTitle));
					query.Add(Expression.Eq("Id", (Int64)1));
					await (query.UniqueResultAsync<ContactTitle>());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					contact.Surname = "Updated surname";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));
					await (session.MergeAsync(contact));
				}
		}

		[Test]
		public async Task DoesNotFlushWithCriteriaWithNeverAsync()
		{
			using (ISession session = OpenSession(new HibernateInterceptor()))
				using (ITransaction tran = session.BeginTransaction())
				{
					session.FlushMode = FlushMode.Never;
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(0));
					Contact contact = await (session.GetAsync<Contact>((Int64)1));
					contact.PreferredName = "Updated preferred name";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					contact.Forename = "Updated forename";
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					ICriteria query = session.CreateCriteria(typeof (ContactTitle));
					query.Add(Expression.Eq("Id", (Int64)1));
					await (query.UniqueResultAsync<ContactTitle>());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					contact.Surname = "Updated surname";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));
					await (session.MergeAsync(contact));
				}
		}

		[Test]
		public async Task DoesNotFlushWithCriteriaWithAutoAsync()
		{
			using (ISession session = OpenSession(new HibernateInterceptor()))
				using (ITransaction tran = session.BeginTransaction())
				{
					session.FlushMode = FlushMode.Auto;
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(0));
					Contact contact = await (session.GetAsync<Contact>((Int64)1));
					contact.PreferredName = "Updated preferred name";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					contact.Forename = "Updated forename";
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));
					ICriteria query = session.CreateCriteria(typeof (ContactTitle));
					query.Add(Expression.Eq("Id", (Int64)1));
					await (query.UniqueResultAsync<ContactTitle>());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));
					contact.Surname = "Updated surname";
					await (session.FlushAsync());
					Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(3));
					await (session.MergeAsync(contact));
				}
		}
	}
}
#endif
