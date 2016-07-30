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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var person = new Person{Children = new HashSet<Person>()};
					await (s.SaveAsync(person));
					var child = new Person();
					await (s.SaveAsync(child));
					person.Children.Add(child);
					await (s.Transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (s.Transaction.CommitAsync());
				}
		}

		[Test]
		public async Task GetGoodErrorForDirtyReassociatedCollectionAsync()
		{
			var ex = Assert.ThrowsAsync<HibernateException>(async () =>
			{
				Person person;
				using (var s = OpenSession())
					using (s.BeginTransaction())
					{
						person = await (s.GetAsync<Person>(1));
						await (NHibernateUtil.InitializeAsync(person.Children));
						await (s.Transaction.CommitAsync());
					}

				person.Children.Clear();
				using (var s = OpenSession())
					using (s.BeginTransaction())
					{
						await (s.LockAsync(person, LockMode.None));
					}
			}

			);
			Assert.That(ex.Message, Is.EqualTo("reassociated object has dirty collection: NHibernate.Test.NHSpecificTest.NH2065.Person.Children"));
		}
	}
}
#endif
