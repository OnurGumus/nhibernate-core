#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Any
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AnyTypeTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"Any.Person.hbm.xml"};
			}
		}

		protected override string CacheConcurrencyStrategy
		{
			get
			{
				return null;
			}
		}

		[Test]
		public async Task FlushProcessingAsync()
		{
			//http://opensource.atlassian.com/projects/hibernate/browse/HHH-1663
			ISession session = OpenSession();
			session.BeginTransaction();
			Person person = new Person();
			Address address = new Address();
			person.Data = address;
			await (session.SaveOrUpdateAsync(person));
			await (session.SaveOrUpdateAsync(address));
			await (session.Transaction.CommitAsync());
			session.Close();
			session = OpenSession();
			session.BeginTransaction();
			person = (Person)await (session.LoadAsync(typeof (Person), person.Id));
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
