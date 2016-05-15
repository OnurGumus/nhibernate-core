#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2959
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUsePolymorphicCriteriaInMultiCriteriaAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var results = await (session.CreateMultiCriteria().Add(session.CreateCriteria(typeof (BaseEntity))).ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0], Has.Count.EqualTo(2));
				}
		}

		[Test]
		public async Task CanUsePolymorphicQueryInMultiQueryAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var results = await (session.CreateMultiQuery().Add(session.CreateQuery("from " + typeof (BaseEntity).FullName)).ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0], Has.Count.EqualTo(2));
				}
		}
	}
}
#endif
