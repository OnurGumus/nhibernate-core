#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1356
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CanLoadWithGenericCompositeElementAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var person = new Person{Name = "Bob", Addresses = NewCollection()};
					person.Addresses.Add(new Address("123 Main St.", "Anytown", "LA", "12345"));
					person.Addresses.Add(new Address("456 Main St.", "Anytown", "LA", "12345"));
					await (session.SaveAsync(person));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				var person = await (session.CreateQuery("from Person").UniqueResultAsync<Person>());
				Assert.IsNotNull(person);
				Assert.IsNotNull(person.Addresses);
				Assert.AreEqual(2, person.Addresses.Count);
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}

		protected abstract ICollection<Address> NewCollection();
	}
}
#endif
