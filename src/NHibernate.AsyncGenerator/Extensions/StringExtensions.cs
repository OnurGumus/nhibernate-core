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
	internal static class StringExtensions
	{
		public static string EscapeIdentifier(
			this string identifier,
			bool isQueryContext = false)
		{
			var nullIndex = identifier.IndexOf('\0');
			if (nullIndex >= 0)
			{
				identifier = identifier.Substring(0, nullIndex);
			}

			var needsEscaping = SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None;

			// Check if we need to escape this contextual keyword
			needsEscaping = needsEscaping || (isQueryContext && SyntaxFacts.IsQueryContextualKeyword(SyntaxFacts.GetContextualKeywordKind(identifier)));

			return needsEscaping ? "@" + identifier : identifier;
		}

		public static SyntaxToken ToIdentifierToken(
			this string identifier,
			bool isQueryContext = false)
		{
			var escaped = identifier.EscapeIdentifier(isQueryContext);

			if (escaped.Length == 0 || escaped[0] != '@')
			{
				return SyntaxFactory.Identifier(escaped);
			}

			var unescaped = identifier.StartsWith("@", StringComparison.Ordinal)
				? identifier.Substring(1)
				: identifier;

			var token = SyntaxFactory.Identifier(
				default(SyntaxTriviaList), SyntaxKind.None, "@" + unescaped, unescaped, default(SyntaxTriviaList));

			if (!identifier.StartsWith("@", StringComparison.Ordinal))
			{
				token = token.WithAdditionalAnnotations(Simplifier.Annotation);
			}

			return token;
		}

		public static IdentifierNameSyntax ToIdentifierName(this string identifier)
		{
			return SyntaxFactory.IdentifierName(identifier.ToIdentifierToken());
		}
	}
}
