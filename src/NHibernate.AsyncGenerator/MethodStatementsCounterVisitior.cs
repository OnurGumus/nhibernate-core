using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public class MethodStatementsCounterVisitior : CSharpSyntaxRewriter
	{
		public List<InvocationExpressionSyntax> InvocationExpressions { get; } = new List<InvocationExpressionSyntax>();

		public int TotalYields { get; private set; }

		public int TotalStatements { get; private set; }

		public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
		{
			InvocationExpressions.Add(node);
			return base.VisitInvocationExpression(node);
		}

		public override SyntaxNode Visit(SyntaxNode node)
		{
			if (!node.IsKind(SyntaxKind.Block) && node is StatementSyntax)
			{
				TotalStatements++;
			}
			return base.Visit(node);
		}

		public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
		{
			TotalYields++;
			return base.VisitYieldStatement(node);
		}
	}
}
