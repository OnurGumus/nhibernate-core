#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Any
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AnyTypeTest : TestCase
	{
		[Test]
		public async Task FlushProcessingAsync()
		{
			//http://opensource.atlassian.com/projects/hibernate/browse/HHH-1663
			ISession session = OpenSession();
			session.BeginTransaction();
			Person person = new Person();
			Address address = new Address();
			person.Data = address;
			session.SaveOrUpdate(person);
			session.SaveOrUpdate(address);
			await (session.Transaction.CommitAsync());
			session.Close();
			session = OpenSession();
			session.BeginTransaction();
			person = (Person)session.Load(typeof (Person), person.Id);
			person.Name = "makingpersondirty";
			await (session.Transaction.CommitAsync());
			session.Close();
			session = OpenSession();
			session.BeginTransaction();
			await (session.DeleteAsync(person));
			await (session.DeleteAsync(address));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
