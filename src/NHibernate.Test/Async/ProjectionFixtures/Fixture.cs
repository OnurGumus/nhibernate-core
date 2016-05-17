#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ProjectionFixtures
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task ErrorFromDBWillGiveTheActualSQLExecutedAsync()
		{
			if (!(Dialect is MsSql2000Dialect))
				Assert.Ignore("Test checks for exact sql and expects an error to occur in a case which is not erroneous on all databases.");
			string pName = ((ISqlParameterFormatter)sessions.ConnectionProvider.Driver).GetParameterName(0);
			string expectedMessagePart0 = string.Format("could not execute query" + Environment.NewLine + "[ SELECT this_.Id as y0_, count(this_.Area) as y1_ FROM TreeNode this_ WHERE this_.Id = {0} ]", pName);
			string expectedMessagePart1 = string.Format(@"[SQL: SELECT this_.Id as y0_, count(this_.Area) as y1_ FROM TreeNode this_ WHERE this_.Id = {0}]", pName);
			DetachedCriteria projection = DetachedCriteria.For<TreeNode>("child").Add(Restrictions.Eq("child.Key.Id", 2)).SetProjection(Projections.ProjectionList().Add(Projections.Property("child.Key.Id")).Add(Projections.Count("child.Key.Area")));
			var e = Assert.Throws<GenericADOException>(async () =>
			{
				using (var s = sessions.OpenSession())
					using (var tx = s.BeginTransaction())
					{
						var criteria = projection.GetExecutableCriteria(s);
						criteria.List();
						await (tx.CommitAsync());
					}
			}

			);
			Assert.That(e.Message, Is.StringContaining(expectedMessagePart0).Or.StringContaining(expectedMessagePart1));
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
					var list = criteria.List();
					Assert.AreEqual(1, list.Count);
					var tuple = (object[])list[0];
					Assert.AreEqual(11, tuple[0]);
					Assert.AreEqual(2, tuple[1]);
					Assert.AreEqual(1, tuple[2]);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
