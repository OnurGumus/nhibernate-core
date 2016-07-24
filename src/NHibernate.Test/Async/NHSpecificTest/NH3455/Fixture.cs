#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3455
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Person{Name = "Bob", Age = 31, Weight = 185, Address = new Address{City = "Abington", State = "VA", Street = "Avenue", Zip = "11121"}};
					await (session.SaveAsync(e1));
					var e2 = new Person{Name = "Sally", Age = 22, Weight = 122, Address = new Address{City = "Olympia", State = "WA", Street = "Broad", Zip = "99989"}};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

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
