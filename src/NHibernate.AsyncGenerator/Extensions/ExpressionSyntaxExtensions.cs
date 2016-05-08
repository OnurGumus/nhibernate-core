using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Simplification;

namespace NHibernate.AsyncGenerator.Extensions
{
	public static class ExpressionSyntaxExtensions
	{
		public static ExpressionSyntax Parenthesize(this ExpressionSyntax expression, bool includeElasticTrivia = true)
		{
			if (includeElasticTrivia)
			{
				return SyntaxFactory.ParenthesizedExpression(expression.WithoutTrivia())
									.WithTriviaFrom(expression)
									.WithAdditionalAnnotations(Simplifier.Annotation);
			}
			else
			{
				return SyntaxFactory.ParenthesizedExpression
				(
					SyntaxFactory.Token(SyntaxTriviaList.Empty, SyntaxKind.OpenParenToken, SyntaxTriviaList.Empty),
					expression.WithoutTrivia(),
					SyntaxFactory.Token(SyntaxTriviaList.Empty, SyntaxKind.CloseParenToken, SyntaxTriviaList.Empty)
				)
				.WithTriviaFrom(expression)
				.WithAdditionalAnnotations(Simplifier.Annotation);
			}
		}
	}
}
