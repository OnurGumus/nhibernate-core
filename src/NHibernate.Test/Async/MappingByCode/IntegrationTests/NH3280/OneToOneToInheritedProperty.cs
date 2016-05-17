#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3280
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OneToOneToInheritedProperty : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldConfigureSessionCorrectlyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var person1 = await (session.GetAsync<Person>(_person1Id));
					var person2 = await (session.GetAsync<Person>(_person2Id));
					var personDetail = await (session.GetAsync<PersonDetail>(_personDetailId));
					Assert.IsNull(person2.PersonDetail);
					Assert.IsNotNull(person1.PersonDetail);
					Assert.AreEqual(person1.PersonDetail.LastName, personDetail.LastName);
					Assert.AreEqual(person1.FirstName, personDetail.Person.FirstName);
				}
		}
	}
}
#endif
