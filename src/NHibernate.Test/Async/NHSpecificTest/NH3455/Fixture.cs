#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3455
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task OrderBySpecifiedPropertyWithQueryOverAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					PersonDto dto = null;
					var people = await (session.QueryOver<Person>().SelectList(b => b.Select(p => p.Id).WithAlias(() => dto.Id).Select(p => p.Name).WithAlias(() => dto.Name).Select(p => p.Address).WithAlias(() => dto.Address).Select(p => p.Age).WithAlias(() => dto.Age)).OrderBy(p => p.Age).Desc.TransformUsing(Transformers.AliasToBean<PersonDto>()).ListAsync<PersonDto>());
					Assert.That(people.Count, Is.EqualTo(2));
					Assert.That(people, Is.Ordered.By("Age").Descending);
				}
		}

		[Test]
		public async Task OrderBySpecifiedPropertyWithCriteriaAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var selectList = Projections.ProjectionList().Add(Projections.Property("Id"), "Id").Add(Projections.Property("Name"), "Name").Add(Projections.Property("Address"), "Address").Add(Projections.Property("Age"), "Age");
					var order = new Order("Age", false);
					var people = await (session.CreateCriteria<Person>().SetProjection(selectList).AddOrder(order).SetResultTransformer(Transformers.AliasToBean<PersonDto>()).ListAsync<PersonDto>());
					Assert.That(people.Count, Is.EqualTo(2));
					Assert.That(people, Is.Ordered.By("Age").Descending);
				}
		}
	}
}
#endif
