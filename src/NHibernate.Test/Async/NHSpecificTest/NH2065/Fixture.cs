#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using NHibernate.Impl;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2065
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		[ExpectedException(ExpectedException = typeof (HibernateException), ExpectedMessage = "reassociated object has dirty collection: NHibernate.Test.NHSpecificTest.NH2065.Person.Children")]
		public async Task GetGoodErrorForDirtyReassociatedCollectionAsync()
		{
			Person person;
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					person = await (s.GetAsync<Person>(1));
					NHibernateUtil.Initialize(person.Children);
					await (s.Transaction.CommitAsync());
				}

			person.Children.Clear();
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					s.Lock(person, LockMode.None);
				}
		}
	}
}
#endif
