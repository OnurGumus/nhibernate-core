#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH315
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH315";
			}
		}

		[Test]
		public async Task SaveClientAsync()
		{
			Client client = new Client();
			Person person = new Person();
			client.Contacts = new ClientPersons();
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(person));
				client.Contacts.PersonId = person.Id;
				client.Contacts.Person = person;
				await (s.SaveAsync(client));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(client));
				await (s.DeleteAsync(person));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
