#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH637
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task MultiColumnBetweenAsync()
		{
			PointHolder holder = new PointHolder();
			holder.Point = new Point(20, 10);
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(holder));
					PointHolder result = (PointHolder)await (s.CreateCriteria(typeof (PointHolder)).Add(Expression.Between("Point", new Point(19, 9), new Point(21, 11))).UniqueResultAsync());
					Assert.AreSame(holder, result);
					await (s.DeleteAsync(holder));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
