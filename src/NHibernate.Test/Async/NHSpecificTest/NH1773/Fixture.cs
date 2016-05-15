#if NET_4_5
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1773
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CustomHQLFunctionsShouldBeRecognizedByTheParserAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Country c = new Country()
					{Id = 100, Name = "US"};
					Person p = new Person()
					{Age = 35, Name = "My Name", Id = 1, Country = c};
					await (s.SaveAsync(c));
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<PersonResult> result = await (s.CreateQuery("select new PersonResult(p, current_timestamp()) from Person p left join fetch p.Country").ListAsync<PersonResult>());
					Assert.AreEqual("My Name", result[0].Person.Name);
					Assert.IsTrue(NHibernateUtil.IsInitialized(result[0].Person.Country));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
