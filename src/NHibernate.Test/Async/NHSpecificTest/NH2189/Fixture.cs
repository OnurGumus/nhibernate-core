#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2189
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private Guid _policy2Id;
		protected override async System.Threading.Tasks.Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					TeamMember tm1 = new TeamMember()
					{Name = "Joe"};
					TeamMember tm2 = new TeamMember()
					{Name = "Bill"};
					await (s.SaveAsync(tm1));
					await (s.SaveAsync(tm2));
					var policy1 = new Policy()
					{PolicyNumber = 5};
					policy1.Tasks.Add(new Task()
					{Policy = policy1, TaskName = "Task1", TeamMember = tm1});
					var policy2 = new Policy()
					{PolicyNumber = 5};
					policy2.Tasks.Add(new Task()
					{Policy = policy2, TaskName = "Task2", TeamMember = tm2});
					policy2.Tasks.Add(new Task()
					{Policy = policy2, TaskName = "Task3", TeamMember = tm2});
					await (s.SaveAsync(policy1));
					await (s.SaveAsync(policy2));
					_policy2Id = policy2.Id;
					await (tx.CommitAsync());
				}
		}

		protected override async System.Threading.Tasks.Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("FROM Task"));
					await (s.DeleteAsync("FROM Policy"));
					await (s.DeleteAsync("FROM TeamMember"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async System.Threading.Tasks.Task FutureQueryReturnsExistingProxyAsync()
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
		public async System.Threading.Tasks.Task FutureCriteriaReturnsExistingProxyAsync()
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
		public async System.Threading.Tasks.Task FutureQueryEagerLoadUsesAlreadyLoadedEntityAsync()
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
		public async System.Threading.Tasks.Task FutureCriteriaEagerLoadUsesAlreadyLoadedEntityAsync()
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
