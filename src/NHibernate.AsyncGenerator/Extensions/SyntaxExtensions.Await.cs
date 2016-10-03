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
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static partial class SyntaxExtensions
	{
		public static TypeDeclarationSyntax AddGeneratedCodeAttribute(this TypeDeclarationSyntax typeDeclaration, bool keepExisting = true)
		{
			// If there are some comments we need to move them before the attribute
			var modifier = typeDeclaration.GetModifierWithLeadingTrivia();
			if (modifier.HasValue)
			{
				typeDeclaration = typeDeclaration.ReplaceToken(modifier.Value, modifier.Value.WithLeadingTrivia(SyntaxTriviaList.Empty));
			}

			var attrList = AttributeList(
				Token(SyntaxKind.OpenBracketToken).WithLeadingTrivia(modifier?.LeadingTrivia ?? SyntaxTriviaList.Empty),
				null,
				SeparatedList(
					new List<AttributeSyntax>
					{
						Attribute(IdentifierName("System.CodeDom.Compiler.GeneratedCode(\"AsyncGenerator\", \"1.0.0\")"))
					}
				),
				Token(SyntaxKind.CloseBracketToken)
			);
			if (keepExisting)
			{
				return typeDeclaration
					.WithAttributes(typeDeclaration.AttributeLists.Add(attrList));
			}
			return typeDeclaration
				.WithAttributes(List(new List<AttributeListSyntax> { attrList }));
		}

		public static bool IsPartial(this TypeDeclarationSyntax typeDeclaration)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null)
			{
				return typeDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword));
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null)
			{
				return typeDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword));
			}
			return false;
		}

		public static TypeDeclarationSyntax AddPartial(this TypeDeclarationSyntax typeDeclaration, Func<SyntaxToken, SyntaxToken> partialTokenFun = null)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null && !typeDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword)))
			{
				var token = Token(SyntaxKind.PartialKeyword);
				token = partialTokenFun?.Invoke(token) ?? token;
				return interfaceDeclaration
					.AddModifiers(token);
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null && !classDeclaration.Modifiers.Any(o => o.IsKind(SyntaxKind.PartialKeyword)))
			{
				var token = Token(SyntaxKind.PartialKeyword);
				token = partialTokenFun?.Invoke(token) ?? token;
				return classDeclaration
					.AddModifiers(token);
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

		public static MethodDeclarationSyntax WithoutAttribute(this MethodDeclarationSyntax methodDeclaration, string name)
		{
			var attr = methodDeclaration.AttributeLists.FirstOrDefault(o => o.Attributes.Any(a => a.Name.ToString() == name));
			return attr != null
				? methodDeclaration.WithAttributeLists(methodDeclaration.AttributeLists.Remove(attr))
				: methodDeclaration;
		}

		public static TypeDeclarationSyntax WithoutAttributes(this TypeDeclarationSyntax typeDeclaration)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null)
			{
				return interfaceDeclaration
					.WithAttributeLists(List<AttributeListSyntax>());
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null)
			{
				return classDeclaration
					.WithAttributeLists(List<AttributeListSyntax>());
			}
			return typeDeclaration;
		}

		public static MethodDeclarationSyntax ReturnAsTask(
			this MethodDeclarationSyntax methodNode,
			IMethodSymbol methodSymbol, bool withFullName)
		{
			var leadingTrivia = methodNode.ReturnType.GetLeadingTrivia();
			if (methodSymbol.ReturnsVoid)
			{
				var taskNode = IdentifierName("Task")
					.WithLeadingTrivia(leadingTrivia);
				return methodNode
					.WithReturnType(
						withFullName
							? QualifiedName(
								QualifiedName(
									QualifiedName(
										IdentifierName("System"),
										IdentifierName("Threading")),
									IdentifierName("Tasks")),
								taskNode)
							: (TypeSyntax) taskNode);
			}
			var genericTaskNode = GenericName("Task")
				.WithLeadingTrivia(leadingTrivia)
				.AddTypeArgumentListArguments(methodNode.ReturnType.WithoutLeadingTrivia());
			return methodNode
				.WithReturnType(
					withFullName
						? QualifiedName(
							QualifiedName(
								QualifiedName(
									IdentifierName("System"),
									IdentifierName("Threading")),
								IdentifierName("Tasks")),
							genericTaskNode)
						: (TypeSyntax) genericTaskNode);
		}

		public static SyntaxNode AddAwait(this SyntaxNode oldNode)
		{
			var expression = oldNode as ExpressionSyntax;
			if (expression == null)
			{
				return default(ExpressionSyntax);
			}
			var awaitNode = ConvertToAwaitExpression(expression);
			var nextToken = expression.Parent.ChildNodesAndTokens().FirstOrDefault(o => o.SpanStart >= expression.Span.End); // token can be in a new line
			if (nextToken.IsKind(SyntaxKind.DotToken) || nextToken.IsKind(SyntaxKind.BracketedArgumentList))
			{
				awaitNode = ParenthesizedExpression(awaitNode);
			}

			return awaitNode;
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

		private static ExpressionSyntax ConvertToAwaitExpression(ExpressionSyntax expression)
		{
			if ((expression is BinaryExpressionSyntax || expression is ConditionalExpressionSyntax) && expression.HasTrailingTrivia)
			{
				var expWithTrailing = expression.WithoutLeadingTrivia();
				var span = expWithTrailing.GetLocation().GetLineSpan().Span;
				if (span.Start.Line == span.End.Line && !expWithTrailing.DescendantTrivia().Any(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)))
				{
					return AwaitExpression(ParenthesizedExpression(expWithTrailing))
										.WithLeadingTrivia(expression.GetLeadingTrivia())
										.WithAdditionalAnnotations(Formatter.Annotation);
				}
			}

			return AwaitExpression(expression.WithoutTrivia().Parenthesize())
								.WithTriviaFrom(expression)
								.WithAdditionalAnnotations(Simplifier.Annotation, Formatter.Annotation);
		}
	}
}
