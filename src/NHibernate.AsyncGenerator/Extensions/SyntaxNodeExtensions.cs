using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static class SyntaxNodeExtensions
	{
		public static IEnumerable<TNode> GetAncestors<TNode>(this SyntaxNode node)
			where TNode : SyntaxNode
		{
			var current = node.Parent;
			while (current != null)
			{
				if (current is TNode)
				{
					yield return (TNode)current;
				}

				current = current is IStructuredTriviaSyntax
					? ((IStructuredTriviaSyntax)current).ParentTrivia.Token.Parent
					: current.Parent;
			}
		}

		public static T RemoveLeadingRegions<T>(this T memberNode) where T : MemberDeclarationSyntax
		{
			// remove all regions as not all methods will be written in the type
			var leadingTrivia = memberNode.GetLeadingTrivia();
			memberNode = memberNode.WithLeadingTrivia(leadingTrivia.RemoveRegions());

			var baseTypeNode = memberNode as BaseTypeDeclarationSyntax;
			if (baseTypeNode == null)
			{
				return memberNode;
			}
			return memberNode
				.ReplaceToken(
					baseTypeNode.CloseBraceToken,
					baseTypeNode.CloseBraceToken
								.WithLeadingTrivia(baseTypeNode.CloseBraceToken.LeadingTrivia.RemoveRegions()));
		}
	}
}
