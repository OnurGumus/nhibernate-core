#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1499
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			Person john = new Person();
			john.Name = "John";
			Document doc1 = new Document();
			doc1.Person = john;
			doc1.Title = "John's Doc";
			Document doc2 = new Document();
			doc2.Title = "Spec";
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(john));
					await (session.SaveAsync(doc1));
					await (session.SaveAsync(doc2));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (session.DeleteAsync("from Document"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CheckIfDetachedCriteriaCanBeUsedOnPropertyRestrictionAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					DetachedCriteria detached = DetachedCriteria.For(typeof (Person)).Add(Property.ForName("Name").Eq("John"));
					ICriteria criteria = session.CreateCriteria(typeof (Document)).Add(Restrictions.Or(Property.ForName("Title").Eq("Spec"), Property.ForName("Person").Eq(detached)));
					Assert.ThrowsAsync<QueryException>(async () => await (criteria.ListAsync<Document>()));
				}
		}
	}
}
#endif
