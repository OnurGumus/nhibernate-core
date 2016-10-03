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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.FormatSql, "false");
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = sessions.OpenSession())
				using (s.BeginTransaction())
				{
					var parent = new Employee{Id = 2, };
					var emp = new Employee{Id = 1, Amounts = new HashSet<Money>{new Money{Amount = 9, Currency = "USD"}, new Money{Amount = 3, Currency = "EUR"}, }, ManagedEmployees = new HashSet<ManagedEmployee>{new ManagedEmployee{Position = "parent", Employee = parent}}};
					await (s.SaveAsync(parent));
					await (s.SaveAsync(emp));
					await (s.Transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = sessions.OpenSession())
				using (s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (s.Transaction.CommitAsync());
				}
		}

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

		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.CriteriaQueryOnComponentCollection.Mappings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}
	}
}
#endif
