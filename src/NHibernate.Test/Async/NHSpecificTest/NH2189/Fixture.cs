#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2189
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task FutureQueryReturnsExistingProxyAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Policy policyProxy = await (s.LoadAsync<Policy>(_policy2Id));
					Assert.That(NHibernateUtil.IsInitialized(policyProxy), Is.False);
					IEnumerable<Policy> futurePolicy = await (s.CreateQuery("FROM Policy p where p.Id = :id").SetParameter("id", _policy2Id).FutureAsync<Policy>());
					Policy queriedPolicy = futurePolicy.ElementAt(0);
					Assert.That(NHibernateUtil.IsInitialized(queriedPolicy));
					Assert.That(queriedPolicy, Is.SameAs(policyProxy));
				}
		}

		[Test]
		public async Task FutureCriteriaReturnsExistingProxyAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Policy policyProxy = await (s.LoadAsync<Policy>(_policy2Id));
					Assert.That(NHibernateUtil.IsInitialized(policyProxy), Is.False);
					IEnumerable<Policy> futurePolicy = await (s.CreateCriteria<Policy>().Add(Restrictions.Eq("Id", _policy2Id)).FutureAsync<Policy>());
					Policy queriedPolicy = futurePolicy.ElementAt(0);
					Assert.That(NHibernateUtil.IsInitialized(queriedPolicy));
					Assert.That(queriedPolicy, Is.SameAs(policyProxy));
				}
		}

		[Test]
		public async Task FutureQueryEagerLoadUsesAlreadyLoadedEntityAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Policy policy2 = await (s.CreateQuery("SELECT p FROM Policy p " + "LEFT JOIN FETCH p.Tasks t " + "WHERE p.Id = :id").SetParameter("id", _policy2Id).UniqueResultAsync<Policy>());
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks));
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks.ElementAt(0)));
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks.ElementAt(1)));
					IEnumerable<Task> tasks = await (s.CreateQuery("SELECT t FROM Task t " + "INNER JOIN FETCH t.TeamMember ORDER BY t.TaskName").FutureAsync<Task>());
					Assert.That(tasks.Count(), Is.EqualTo(3));
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(0).TeamMember), Is.True, "Task1 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(1).TeamMember), Is.True, "Task2 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(2).TeamMember), Is.True, "Task3 TeamMember not initialized");
				}
		}

		[Test]
		public async Task FutureCriteriaEagerLoadUsesAlreadyLoadedEntityAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Policy policy2 = await (s.CreateCriteria<Policy>().Add(Restrictions.Eq("Id", _policy2Id)).SetFetchMode("Tasks", FetchMode.Eager).UniqueResultAsync<Policy>());
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks));
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks.ElementAt(0)));
					Assert.That(NHibernateUtil.IsInitialized(policy2.Tasks.ElementAt(1)));
					IEnumerable<Task> tasks = await (s.CreateCriteria<Task>().SetFetchMode("TeamMember", FetchMode.Eager).AddOrder(Order.Asc("TaskName")).FutureAsync<Task>());
					Assert.That(tasks.Count(), Is.EqualTo(3));
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(0).TeamMember), Is.True, "Task1 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(1).TeamMember), Is.True, "Task2 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(2).TeamMember), Is.True, "Task3 TeamMember not initialized");
				}
		}
	}
}
#endif
