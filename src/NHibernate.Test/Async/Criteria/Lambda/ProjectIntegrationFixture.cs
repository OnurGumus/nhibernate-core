#if NET_4_5
using System.Collections;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria.Lambda
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProjectIntegrationFixtureAsync : TestCaseAsync
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
				return new[]{"Criteria.Lambda.Mappings.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person{Name = "test person 1", Age = 20}));
					await (s.SaveAsync(new Person{Name = "test person 1", Age = 30}));
					await (s.SaveAsync(new Person{Name = "test person 2", Age = 40}));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Person").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task MultiplePropertiesAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					Person personAlias = null;
					var actual = (await (s.QueryOver<Person>(() => personAlias).Select(p => p.Name, p => personAlias.Age).OrderBy(p => p.Age).Asc.ListAsync<object[]>())).Select(props => new
					{
					TestName = (string)props[0], TestAge = (int)props[1], }

					);
					Assert.That(actual.ElementAt(0).TestName, Is.EqualTo("test person 1"));
					Assert.That(actual.ElementAt(1).TestAge, Is.EqualTo(30));
				}
		}

		[Test]
		public async Task SinglePropertyAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var actual = await (s.QueryOver<Person>().Select(p => p.Age).OrderBy(p => p.Age).Asc.ListAsync<int>());
					Assert.That(actual[0], Is.EqualTo(20));
				}
		}

		[Test]
		public async Task ProjectTransformToDtoAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					PersonSummary summary = null;
					var actual = await (s.QueryOver<Person>().SelectList(list => list.SelectGroup(p => p.Name).WithAlias(() => summary.Name).Select(Projections.RowCount()).WithAlias(() => summary.Count)).OrderByAlias(() => summary.Name).Asc.TransformUsing(Transformers.AliasToBean<PersonSummary>()).ListAsync<PersonSummary>());
					Assert.That(actual.Count, Is.EqualTo(2));
					Assert.That(actual[0].Name, Is.EqualTo("test person 1"));
					Assert.That(actual[0].Count, Is.EqualTo(2));
					Assert.That(actual[1].Name, Is.EqualTo("test person 2"));
					Assert.That(actual[1].Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
