#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3041
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OneToOneToPropertyReference : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldConfigureSessionCorrectlyAsync()
		{
			using (var session = OpenSession())
			{
				var person1 = await (session.GetAsync<Person>(1));
				var person2 = await (session.GetAsync<Person>(2));
				var personDetail = session.Query<PersonDetail>().Single();
				Assert.IsNull(person2.PersonDetail);
				Assert.IsNotNull(person1.PersonDetail);
				Assert.AreEqual(person1.PersonDetail.LastName, personDetail.LastName);
				Assert.AreEqual(person1.FirstName, personDetail.Person.FirstName);
			}
		}
	}
}
#endif
