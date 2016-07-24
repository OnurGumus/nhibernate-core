#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2331
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Nh2331TestAsync : BugTestCaseAsync
	{
		private Guid person0Id;
		private Guid person1Id;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			var person0 = new Person{Name = "Schorsch", };
			var person1 = new Person{Name = "Sepp", };
			var person2 = new Person{Name = "Detlef", };
			var forum0 = new Forum{Name = "Oof", Dollars = 1887.00, };
			var forum1 = new Forum{Name = "Rab", Dollars = 33.00, };
			var forum2 = new Forum{Name = "Main", Dollars = 42.42, };
			var group0 = new MemberGroup{Name = "Gruppe Bla", Members = new List<Person>(), Forums = new List<Forum>(), };
			group0.Members.Add(person0);
			group0.Forums.Add(forum0);
			group0.Forums.Add(forum1);
			var group1 = new MemberGroup{Name = "Gruppe Blub", Members = new List<Person>(), Forums = new List<Forum>(), };
			group1.Members.Add(person1);
			group1.Members.Add(person2);
			group1.Forums.Add(forum2);
			using (ISession session = OpenSession())
			{
				person0Id = (Guid)await (session.SaveAsync(person0));
				person1Id = (Guid)await (session.SaveAsync(person1));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				string hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return (true);
		}

		[Test]
		public async Task DetachedCriteriaCorrelatedQueryExplodesAsync()
		{
			using (ISession session = OpenSession())
			{
				DetachedCriteria memberGroupCriteria = DetachedCriteria.For<MemberGroup>().CreateAlias("Members", "m").CreateAlias("Forums", "f").Add(Restrictions.EqProperty("m.Id", "p.Id")).SetProjection(Projections.Property("f.Id"));
				var ids = new List<Guid>();
				ids.Add(person0Id);
				ids.Add(person1Id);
				DetachedCriteria forumCriteria = DetachedCriteria.For<Forum>("fff").Add(Restrictions.NotEqProperty("Id", "p.Id")).Add(Subqueries.PropertyIn("Id", memberGroupCriteria)).SetProjection(Projections.Sum("Dollars"));
				DetachedCriteria personCriteria = DetachedCriteria.For<Person>("p").Add(Restrictions.InG("Id", ids)).SetProjection(Projections.ProjectionList().Add(Projections.Property("Name"), "Name").Add(Projections.SubQuery(forumCriteria), "Sum")).SetResultTransformer(Transformers.AliasToBean(typeof (Bar)));
				ICriteria criteria = personCriteria.GetExecutableCriteria(session);
				Assert.That(async () => await (criteria.ListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
