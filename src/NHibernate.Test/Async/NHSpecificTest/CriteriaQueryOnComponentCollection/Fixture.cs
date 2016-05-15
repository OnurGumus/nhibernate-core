#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.CriteriaQueryOnComponentCollection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task CanQueryByCriteriaOnSetOfCompositeElementAsync()
		{
			using (var s = sessions.OpenSession())
			{
				var list = await (s.CreateCriteria<Employee>().CreateCriteria("ManagedEmployees").Add(Restrictions.Eq("Position", "parent")).SetResultTransformer(new RootEntityResultTransformer()).ListAsync());
				Assert.That(list, Has.Count.EqualTo(1));
				Assert.That(list[0], Is.Not.Null);
				Assert.That(list[0], Is.TypeOf<Employee>());
				Assert.That(((Employee)list[0]).Id, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task CanQueryByCriteriaOnSetOfElementAsync()
		{
			using (var s = sessions.OpenSession())
			{
				var list = await (s.CreateCriteria<Employee>().CreateCriteria("Amounts").Add(Restrictions.Gt("Amount", 5m)).SetResultTransformer(new RootEntityResultTransformer()).ListAsync());
				Assert.That(list, Has.Count.EqualTo(1));
				Assert.That(list[0], Is.Not.Null);
				Assert.That(list[0], Is.TypeOf<Employee>());
				Assert.That(((Employee)list[0]).Id, Is.EqualTo(1));
			}
		}

		[TestCase(JoinType.LeftOuterJoin)]
		[TestCase(JoinType.InnerJoin)]
		public async Task CanQueryByCriteriaOnSetOfElementByCreateAliasAsync(JoinType joinType)
		{
			using (var s = sessions.OpenSession())
			{
				var list = await (s.CreateCriteria<Employee>("x").CreateAlias("x.Amounts", "amount", joinType).Add(Restrictions.Gt("amount.Amount", 5m)).SetResultTransformer(new RootEntityResultTransformer()).ListAsync());
				Assert.That(list, Has.Count.EqualTo(1));
				Assert.That(list[0], Is.Not.Null);
				Assert.That(list[0], Is.TypeOf<Employee>());
				Assert.That(((Employee)list[0]).Id, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task CanQueryByCriteriaOnSetOfCompositeElement_UsingDetachedCriteriaAsync()
		{
			using (var s = sessions.OpenSession())
			{
				var list = await (s.CreateCriteria<Employee>().Add(Subqueries.PropertyIn("id", DetachedCriteria.For<Employee>().SetProjection(Projections.Id()).CreateCriteria("Amounts").Add(Restrictions.Gt("Amount", 5m)))).ListAsync());
				Assert.That(list, Has.Count.EqualTo(1));
				Assert.That(list[0], Is.Not.Null);
				Assert.That(list[0], Is.TypeOf<Employee>());
				Assert.That(((Employee)list[0]).Id, Is.EqualTo(1));
			}
		}
	}
}
#endif
