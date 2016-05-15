#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ProjectionFixtures
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public Task ErrorFromDBWillGiveTheActualSQLExecutedAsync()
		{
			try
			{
				ErrorFromDBWillGiveTheActualSQLExecuted();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task AggregatingHirearchyWithCountAsync()
		{
			var root = new Key{Id = 1, Area = 2};
			DetachedCriteria projection = DetachedCriteria.For<TreeNode>("child").Add(Restrictions.Eq("Parent.id", root)).Add(Restrictions.Gt("Key.Id", 0)).Add(Restrictions.Eq("Type", NodeType.Blue)).CreateAlias("DirectChildren", "grandchild").SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty("child.Key.Id")).Add(Projections.GroupProperty("child.Key.Area")).Add(Projections.Count(Projections.Property("grandchild.Key.Id"))));
			using (var s = sessions.OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var criteria = projection.GetExecutableCriteria(s);
					var list = await (criteria.ListAsync());
					Assert.AreEqual(1, list.Count);
					var tuple = (object[])list[0];
					Assert.AreEqual(11, tuple[0]);
					Assert.AreEqual(2, tuple[1]);
					Assert.AreEqual(1, tuple[2]);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task LimitingResultSetOnQueryThatIsOrderedByProjectionAsync()
		{
			using (var s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (TreeNode), "parent").Add(Restrictions.Gt("Key.Id", 0));
				var currentAssessment = DetachedCriteria.For<TreeNode>("child").Add(Restrictions.EqProperty("Key.Id", "parent.Key.Id")).Add(Restrictions.EqProperty("Key.Area", "parent.Key.Area")).Add(Restrictions.Eq("Type", NodeType.Smart)).SetProjection(Projections.Property("Type"));
				criteria.AddOrder(Order.Asc(Projections.SubQuery(currentAssessment))).SetMaxResults(1000);
				await (criteria.ListAsync());
			}
		}

		[Test]
		public async Task QueryingWithParemetersAndParaemtersInOrderByAsync()
		{
			using (var s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (TreeNode), "parent").Add(Restrictions.Like("Name", "ayende")).Add(Restrictions.Gt("Key.Id", 0));
				var currentAssessment = DetachedCriteria.For<TreeNode>("child").Add(Restrictions.Eq("Type", NodeType.Smart)).SetProjection(Projections.Property("Type"));
				criteria.AddOrder(Order.Asc(Projections.SubQuery(currentAssessment))).SetMaxResults(1000);
				await (criteria.ListAsync());
			}
		}
	}
}
#endif
