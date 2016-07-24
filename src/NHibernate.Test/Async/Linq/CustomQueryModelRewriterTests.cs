#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Linq.Visitors;
using NUnit.Framework;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomQueryModelRewriterTestsAsync : LinqTestCaseAsync
	{
		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			configuration.Properties[Cfg.Environment.QueryModelRewriterFactory] = typeof (QueryModelRewriterFactory).AssemblyQualifiedName;
			return TaskHelper.CompletedTask;
		}

		[Test]
		public void RewriteNullComparison()
		{
			// This example shows how to use the query model rewriter to
			// make radical changes to the query. In this case, we rewrite
			// a null comparison (which would translate into a IS NULL)
			// into a comparison to "Thomas Hardy" (which translates to a = "Thomas Hardy").
			var contacts = (
				from c in db.Customers
				where c.ContactName == null
				select c).ToList();
			Assert.Greater(contacts.Count, 0);
			Assert.IsTrue(contacts.Select(customer => customer.ContactName).All(c => c == "Thomas Hardy"));
		}

		[Serializable]
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class QueryModelRewriterFactory : IQueryModelRewriterFactory
		{
			public QueryModelVisitorBase CreateVisitor(VisitorParameters parameters)
			{
				return new CustomVisitor();
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class CustomVisitor : QueryModelVisitorBase
		{
			public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
			{
				whereClause.TransformExpressions(new Visitor().VisitExpression);
			}

			[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
			private class Visitor : ExpressionTreeVisitor
			{
				protected override Expression VisitBinaryExpression(BinaryExpression expression)
				{
					if (expression.NodeType == ExpressionType.Equal || expression.NodeType == ExpressionType.NotEqual)
					{
						var left = expression.Left;
						var right = expression.Right;
						bool reverse = false;
						if (!(left is ConstantExpression) && right is ConstantExpression)
						{
							var tmp = left;
							left = right;
							right = tmp;
							reverse = true;
						}

						var constant = left as ConstantExpression;
						if (constant != null && constant.Value == null)
						{
							left = Expression.Constant("Thomas Hardy");
							expression = Expression.MakeBinary(expression.NodeType, reverse ? right : left, reverse ? left : right);
						}
					}

					return base.VisitBinaryExpression(expression);
				}
			}
		}
	}
}
#endif
