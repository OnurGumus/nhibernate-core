﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static partial class SyntaxExtensions
	{
		public static T WithPrependedLeadingTrivia<T>(
			this T node,
			params SyntaxTrivia[] trivia) where T : SyntaxNode
		{
			if (trivia.Length == 0)
			{
				return node;
			}

			return node.WithPrependedLeadingTrivia((IEnumerable<SyntaxTrivia>)trivia);
		}

		public static T WithPrependedLeadingTrivia<T>(
			this T node,
			SyntaxTriviaList trivia) where T : SyntaxNode
		{
			if (trivia.Count == 0)
			{
				return node;
			}

			return node.WithLeadingTrivia(trivia.Concat(node.GetLeadingTrivia()));
		}

		public static T WithPrependedLeadingTrivia<T>(
			this T node,
			IEnumerable<SyntaxTrivia> trivia) where T : SyntaxNode
		{
			return node.WithPrependedLeadingTrivia(trivia.ToSyntaxTriviaList());
		}

		public static T WithAppendedTrailingTrivia<T>(
			this T node,
			params SyntaxTrivia[] trivia) where T : SyntaxNode
		{
			if (trivia.Length == 0)
			{
				return node;
			}

			return node.WithAppendedTrailingTrivia((IEnumerable<SyntaxTrivia>)trivia);
		}

		public static T WithAppendedTrailingTrivia<T>(
			this T node,
			SyntaxTriviaList trivia) where T : SyntaxNode
		{
			if (trivia.Count == 0)
			{
				return node;
			}

			return node.WithTrailingTrivia(node.GetTrailingTrivia().Concat(trivia));
		}

		public static T WithAppendedTrailingTrivia<T>(
			this T node,
			IEnumerable<SyntaxTrivia> trivia) where T : SyntaxNode
		{
			return node.WithAppendedTrailingTrivia(trivia.ToSyntaxTriviaList());
		}

		public static T With<T>(
			this T node,
			IEnumerable<SyntaxTrivia> leadingTrivia,
			IEnumerable<SyntaxTrivia> trailingTrivia) where T : SyntaxNode
		{
			return node.WithLeadingTrivia(leadingTrivia).WithTrailingTrivia(trailingTrivia);
		}

		public static SyntaxTriviaList RemoveRegions(this SyntaxTriviaList list)
		{
			var toRemove = new List<int>();
			for (var i = list.Count - 1; i >= 0; i--)
			{
				var trivia = list[i];
				if (trivia.IsKind(SyntaxKind.RegionDirectiveTrivia) || trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia))
				{
					toRemove.Add(i);
				}
			}
			while (toRemove.Count > 0)
			{
				list = list.RemoveAt(toRemove[0]);
				toRemove.RemoveAt(0);
			}
			return list;
		}
	}
}
