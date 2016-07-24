#if NET_4_5
using NHibernate.Util;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ExpressionsHelperFixtureAsync
	{
		[Test]
		public void DecodeMemberAccessExpression()
		{
			Assert.That(ExpressionsHelper.DecodeMemberAccessExpression<TestingClass, int>(x => x.IntProp), Is.EqualTo(typeof (TestingClass).GetMember("IntProp")[0]));
			Assert.That(ExpressionsHelper.DecodeMemberAccessExpression<TestingClass, bool>(x => x.BoolProp), Is.EqualTo(typeof (TestingClass).GetMember("BoolProp")[0]));
			Assert.That(ExpressionsHelper.DecodeMemberAccessExpression<TestingClass, IEnumerable<string>>(x => x.CollectionProp), Is.EqualTo(typeof (TestingClass).GetMember("CollectionProp")[0]));
		}
	}
}
#endif
