#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1356
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class Fixture : BugTestCase
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
				var person = session.CreateQuery("from Person").UniqueResult<Person>();
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
	}
}
#endif
