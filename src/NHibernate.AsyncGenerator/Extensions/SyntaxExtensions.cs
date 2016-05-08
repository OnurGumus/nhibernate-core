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
