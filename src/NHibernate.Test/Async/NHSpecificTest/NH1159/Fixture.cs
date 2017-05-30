﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1159
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync:BugTestCase
	{

		protected override void  OnSetUp()
		{
			using (ISession session = OpenSession())
			using (ITransaction tran = session.BeginTransaction())
			{
				Contact c=new Contact{Id=1,Forename ="David",Surname="Bates",PreferredName="Davey"};
				session.Save(c);
				tran.Commit();
			}
			HibernateInterceptor.CallCount = 0;

        }

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			using (ITransaction tran = session.BeginTransaction())
			{
				session.Delete("from Contact");
				tran.Commit();
			}
		}

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

				ICriteria query = session.CreateCriteria(typeof(ContactTitle));
				query.Add(Expression.Eq("Id", (Int64)1));
				await (query.UniqueResultAsync<ContactTitle>());

				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));

				contact.Surname = "Updated surname";
				await (session.FlushAsync());
				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));

				session.Merge(contact);
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

				ICriteria query = session.CreateCriteria(typeof(ContactTitle));
				query.Add(Expression.Eq("Id", (Int64)1));
				await (query.UniqueResultAsync<ContactTitle>());

				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(1));

				contact.Surname = "Updated surname";
				await (session.FlushAsync());
				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));

				session.Merge(contact);
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

				ICriteria query = session.CreateCriteria(typeof(ContactTitle));
				query.Add(Expression.Eq("Id", (Int64)1));
				await (query.UniqueResultAsync<ContactTitle>());

				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(2));

				contact.Surname = "Updated surname";
				await (session.FlushAsync());
				Assert.That(HibernateInterceptor.CallCount, Is.EqualTo(3));

				session.Merge(contact);
			}
		}
	}
}
