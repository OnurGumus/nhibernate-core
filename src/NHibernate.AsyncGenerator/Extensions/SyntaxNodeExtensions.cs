using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

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
	}
}
