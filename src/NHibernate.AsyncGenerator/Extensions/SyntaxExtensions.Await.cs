using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static partial class SyntaxExtensions
	{
		public static TypeDeclarationSyntax AddPartial(this TypeDeclarationSyntax typeDeclaration)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null && !typeDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword)))
			{
				return interfaceDeclaration
					.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null && !classDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword)))
			{
				return classDeclaration
					.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
			}
			return typeDeclaration;
		}

		public static TypeDeclarationSyntax WithMembers(this TypeDeclarationSyntax typeDeclaration, SyntaxList<MemberDeclarationSyntax> members)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null)
			{
				return interfaceDeclaration
					.WithMembers(members);
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null)
			{
				return classDeclaration
					.WithMembers(members);
			}
			return typeDeclaration;
		}

		public static TypeDeclarationSyntax WithAttributes(this TypeDeclarationSyntax typeDeclaration, SyntaxList<AttributeListSyntax> attributes)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null)
			{
				return interfaceDeclaration
					.WithAttributeLists(attributes);
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null)
			{
				return classDeclaration
					.WithAttributeLists(attributes);
			}
			return typeDeclaration;
		}

		public static MethodDeclarationSyntax ReturnAsTask(
			this MethodDeclarationSyntax methodNode,
			IMethodSymbol methodSymbol,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			if (methodSymbol.ReturnsVoid)
			{
				return methodNode.WithReturnType(SyntaxFactory.IdentifierName("Task")).NormalizeWhitespace();
			}
			return methodNode.WithReturnType(SyntaxFactory.GenericName("Task").AddTypeArgumentListArguments(methodNode.ReturnType)).NormalizeWhitespace();
		}

		public static T AddAwait<T>(
			this SyntaxNode oldNode,
			T root,
			CancellationToken cancellationToken = default(CancellationToken)) where T : SyntaxNode
		{
			var expression = oldNode as ExpressionSyntax;
			if (expression == null)
			{
				return default(T);
			}
			return root.ReplaceNode(oldNode, ConvertToAwaitExpression(expression));
		}

		private static bool IsInAsyncFunction(ExpressionSyntax expression)
		{
			foreach (var node in expression.Ancestors())
			{
				switch (node.Kind())
				{
					case SyntaxKind.ParenthesizedLambdaExpression:
					case SyntaxKind.SimpleLambdaExpression:
					case SyntaxKind.AnonymousMethodExpression:
						return (node as AnonymousFunctionExpressionSyntax)?.AsyncKeyword.IsMissing == false;
					case SyntaxKind.MethodDeclaration:
						return (node as MethodDeclarationSyntax)?.Modifiers.Any(SyntaxKind.AsyncKeyword) == true;
					default:
						continue;
				}
			}

			return false;
		}

		private static SyntaxNode ConvertToAwaitExpression(ExpressionSyntax expression)
		{
			if ((expression is BinaryExpressionSyntax || expression is ConditionalExpressionSyntax) && expression.HasTrailingTrivia)
			{
				var expWithTrailing = expression.WithoutLeadingTrivia();
				var span = expWithTrailing.GetLocation().GetLineSpan().Span;
				if (span.Start.Line == span.End.Line && !expWithTrailing.DescendantTrivia().Any(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)))
				{
					return SyntaxFactory.AwaitExpression(SyntaxFactory.ParenthesizedExpression(expWithTrailing))
										.WithLeadingTrivia(expression.GetLeadingTrivia())
										.WithAdditionalAnnotations(Formatter.Annotation);
				}
			}

			return SyntaxFactory.AwaitExpression(expression.WithoutTrivia().Parenthesize())
								.WithTriviaFrom(expression)
								.WithAdditionalAnnotations(Simplifier.Annotation, Formatter.Annotation);
		}
	}
}
