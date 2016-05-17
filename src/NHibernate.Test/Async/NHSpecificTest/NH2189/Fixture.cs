#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2189
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
					IEnumerable<Task> tasks = s.CreateCriteria<Task>().SetFetchMode("TeamMember", FetchMode.Eager).AddOrder(Order.Asc("TaskName")).Future<Task>();
					Assert.That(tasks.Count(), Is.EqualTo(3));
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(0).TeamMember), Is.True, "Task1 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(1).TeamMember), Is.True, "Task2 TeamMember not initialized");
					Assert.That(NHibernateUtil.IsInitialized(tasks.ElementAt(2).TeamMember), Is.True, "Task3 TeamMember not initialized");
				}
		}
	}
}
#endif
