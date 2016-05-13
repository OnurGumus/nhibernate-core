using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static partial class SyntaxExtensions
	{
		public static bool IsReturned(this SyntaxNode node)
		{
			if (node.IsKind(SyntaxKind.ReturnStatement))
			{
				return false;
			}
			var statement = node.Ancestors().OfType<StatementSyntax>().FirstOrDefault();
			if (statement == null || !statement.IsKind(SyntaxKind.ReturnStatement))
			{
				return false;
			}
			var currNode = node;
			var totalInvocations = 0;
			var totalSimpleMemberAccesses = 0;
			while (!currNode.IsKind(SyntaxKind.ReturnStatement))
			{
				switch (currNode.Kind())
				{
					case SyntaxKind.ConditionalExpression:
						var conditionExpression = (ConditionalExpressionSyntax)currNode;
						if (IsAsyncMethodInvoked(conditionExpression.WhenTrue as InvocationExpressionSyntax) ||
							IsAsyncMethodInvoked(conditionExpression.WhenFalse as InvocationExpressionSyntax))
						{
							return false;
						}
						break;
				}
				currNode = currNode.Parent;
				switch (currNode.Kind())
				{
					case SyntaxKind.ConditionalExpression:
						var conditionExpression = (ConditionalExpressionSyntax)currNode;
						if (conditionExpression.Condition.Contains(node))
						{
							return false;
						}
						return true;
					case SyntaxKind.SimpleMemberAccessExpression:
						if (totalSimpleMemberAccesses > 0)
						{
							return false;
						}
						totalSimpleMemberAccesses++;
						break;
					case SyntaxKind.InvocationExpression:
						if (IsAsyncMethodInvoked(currNode as InvocationExpressionSyntax) || totalInvocations > 0)
						{
							return false;
						}
						totalInvocations++;
						break;
					case SyntaxKind.ReturnStatement:
						return true;
					default:
						return false;
				}
			}

			return true;
		}

		public static bool IsAsyncMethodInvoked(this InvocationExpressionSyntax node)
		{
			if (node == null)
			{
				return false;
			}
			var identifierNode = node.Expression as SimpleNameSyntax;
			if (identifierNode != null && identifierNode.Identifier.ValueText.EndsWith("Async"))
			{
				return true;
			}
			var memberAccessNode = node.Expression as MemberAccessExpressionSyntax;
			if (memberAccessNode != null && memberAccessNode.ChildNodes().OfType<SimpleNameSyntax>().Last().Identifier.ValueText.EndsWith("Async"))
			{
				return true;
			}
			return false;
		}

		public static bool EndsWithReturnStatement(this BaseMethodDeclarationSyntax node)
		{
			var lastStatement = node.Body.DescendantNodes()
									.Where(o => !(o is BlockSyntax))
									.OfType<StatementSyntax>()
									.LastOrDefault();
			return lastStatement?.IsKind(SyntaxKind.ReturnStatement) == true;
		}

		public static SyntaxToken? GetModifierWithLeadingTrivia(this TypeDeclarationSyntax typeDeclaration)
		{
			var interfaceDeclaration = typeDeclaration as InterfaceDeclarationSyntax;
			if (interfaceDeclaration != null)
			{
				var modifier = interfaceDeclaration.Modifiers.FirstOrDefault();
				if (modifier.HasLeadingTrivia)
				{
					return modifier;
				}
			}
			var classDeclaration = typeDeclaration as ClassDeclarationSyntax;
			if (classDeclaration != null)
			{
				var modifier = classDeclaration.Modifiers.FirstOrDefault();
				if (modifier.HasLeadingTrivia)
				{
					return modifier;
				}
			}
			return null;
		}

		public static SyntaxList<T> RemoveRange<T>(this SyntaxList<T> syntaxList, int index, int count) where T : SyntaxNode
		{
			var result = new List<T>(syntaxList);
			result.RemoveRange(index, count);
			return SyntaxFactory.List(result);
		}

		public static SyntaxList<T> ToSyntaxList<T>(this IEnumerable<T> sequence) where T : SyntaxNode
		{
			return SyntaxFactory.List(sequence);
		}

		public static SyntaxList<T> Insert<T>(this SyntaxList<T> list, int index, T item) where T : SyntaxNode
		{
			return list.Take(index).Concat(item).Concat(list.Skip(index)).ToSyntaxList();
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T value)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			return source.ConcatWorker(value);
		}

		private static IEnumerable<T> ConcatWorker<T>(this IEnumerable<T> source, T value)
		{
			foreach (var v in source)
			{
				yield return v;
			}

			yield return value;
		}
	}
}
