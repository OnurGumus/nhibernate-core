#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class RestrictionsFixtureAsync
	{
		[Test]
		public void LikeShouldContainsMatch()
		{
			ICriterion c = Restrictions.Like("Name", "n", MatchMode.Anywhere, null);
			Assert.That(c, Is.InstanceOf<LikeExpression>());
			var likeExpression = (LikeExpression)c;
			Assert.That(likeExpression.ToString(), Is.EqualTo("Name like %n%"));
		}
	}
}
#endif
