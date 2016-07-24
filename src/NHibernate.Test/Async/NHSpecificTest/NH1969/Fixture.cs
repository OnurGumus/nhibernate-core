#if NET_4_5
using System.Collections.Generic;
using System.IO;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1969
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var entity = new EntityWithTypeProperty{Id = 1, TypeValue = typeof (File)};
					await (s.SaveAsync(entity));
					entity = new EntityWithTypeProperty{Id = 2, TypeValue = typeof (DummyEntity)};
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from DummyEntity").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from EntityWithTypeProperty").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task TestMappedTypeCriteriaAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (EntityWithTypeProperty));
				criteria.Add(Restrictions.Eq("TypeValue", typeof (DummyEntity)));
				IList<EntityWithTypeProperty> results = await (criteria.ListAsync<EntityWithTypeProperty>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(2, results[0].Id);
			}
		}

		[Test]
		public async Task TestMappedTypeHQLAsync()
		{
			using (ISession s = OpenSession())
			{
				IQuery q = s.CreateQuery("select t from EntityWithTypeProperty as t where t.TypeValue = :type");
				q.SetParameter("type", typeof (DummyEntity));
				IList<EntityWithTypeProperty> results = await (q.ListAsync<EntityWithTypeProperty>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(2, results[0].Id);
			}
		}

		[Test]
		public async Task TestNonMappedTypeCriteriaAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (EntityWithTypeProperty));
				criteria.Add(Restrictions.Eq("TypeValue", typeof (File)));
				IList<EntityWithTypeProperty> results = await (criteria.ListAsync<EntityWithTypeProperty>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(1, results[0].Id);
			}
		}

		[Test]
		public async Task TestNonMappedTypeHQLAsync()
		{
			using (ISession s = OpenSession())
			{
				IQuery q = s.CreateQuery("select t from EntityWithTypeProperty as t where t.TypeValue = :type");
				q.SetParameter("type", typeof (File));
				IList<EntityWithTypeProperty> results = await (q.ListAsync<EntityWithTypeProperty>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(1, results[0].Id);
			}
		}
	}
}
#endif
