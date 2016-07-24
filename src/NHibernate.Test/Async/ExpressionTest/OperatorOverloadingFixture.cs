#if NET_4_5
using System;
using System.Reflection;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OperatorOverloadingFixtureAsync
	{
		[Test]
		public void CanUseAndOperatorForExpressions()
		{
			AbstractCriterion lhs = Expression.Eq("foo", "bar");
			AbstractCriterion rhs = Expression.Gt("date", DateTime.Today);
			AbstractCriterion and = lhs && rhs;
			Assert.AreEqual(typeof (AndExpression), and.GetType());
			Assert.AreEqual(lhs, GetLeftSide(and));
			Assert.AreEqual(rhs, GetRightSide(and));
		}

		[Test]
		public void CanUseOrOperatorForEpressions()
		{
			AbstractCriterion lhs = Expression.Eq("foo", "bar");
			AbstractCriterion rhs = Expression.Gt("date", DateTime.Today);
			AbstractCriterion or = lhs || rhs;
			Assert.AreEqual(typeof (OrExpression), or.GetType());
			Assert.AreEqual(lhs, GetLeftSide(or));
			Assert.AreEqual(rhs, GetRightSide(or));
		}

		[Test]
		public void CanUseNotOperatorForExpressions()
		{
			AbstractCriterion lhs = Expression.Eq("foo", "bar");
			AbstractCriterion not = !lhs;
			Assert.AreEqual(typeof (NotExpression), not.GetType());
			AbstractCriterion not_value = (AbstractCriterion)GetField("_criterion", typeof (NotExpression)).GetValue(not);
			Assert.AreEqual(lhs, not_value);
		}

		private static AbstractCriterion GetLeftSide(AbstractCriterion criterion)
		{
			return (AbstractCriterion)GetField("_lhs", typeof (LogicalExpression)).GetValue(criterion);
		}

		private static FieldInfo GetField(string lhs, System.Type type)
		{
			return type.GetField(lhs, BindingFlags.Instance | BindingFlags.NonPublic);
		}

		private static AbstractCriterion GetRightSide(AbstractCriterion criterion)
		{
			return (AbstractCriterion)GetField("_rhs", typeof (LogicalExpression)).GetValue(criterion);
		}
	}
}
#endif
