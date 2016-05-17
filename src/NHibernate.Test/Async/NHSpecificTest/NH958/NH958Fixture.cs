#if NET_4_5
using System;
using NHibernate;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH958
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH958Fixture : BugTestCase
	{
		[Test]
		public async Task MergeWithAny1Async()
		{
			Person person;
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					person = new Male("Test");
					for (int i = 0; i < 10; i++)
					{
						person.AddHobby(new Hobby("Hobby_" + i.ToString()));
					}

					session.SaveOrUpdate(person);
					await (transaction.CommitAsync());
				}

			person.Hobbies.Clear();
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					person = session.Merge(person);
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync(person));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task MergeWithAny2Async()
		{
			Person person;
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					person = new Male("Test");
					await (session.SaveAsync(person));
					await (transaction.CommitAsync());
				}

			person.AddHobby(new Hobby("Hobby_1"));
			person.AddHobby(new Hobby("Hobby_2"));
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					// the transient hobby "test" is inserted and updated
					person = session.Merge(person);
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync(person));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
