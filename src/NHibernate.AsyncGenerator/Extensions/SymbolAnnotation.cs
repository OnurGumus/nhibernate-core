using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace NHibernate.AsyncGenerator.Extensions
{
	/// <summary>
	/// An annotation that holds onto information about a type or namespace symbol.
	/// </summary>
	internal class SymbolAnnotation
	{
		public const string Kind = "SymbolId";

		public static SyntaxAnnotation Create(ISymbol symbol)
		{
			return new SyntaxAnnotation(Kind, DocumentationCommentId.CreateReferenceId(symbol));
		}

		public static ISymbol GetSymbol(SyntaxAnnotation annotation, Compilation compilation)
		{
			return GetSymbols(annotation, compilation).FirstOrDefault();
		}

		public static IEnumerable<ISymbol> GetSymbols(SyntaxAnnotation annotation, Compilation compilation)
		{
			return DocumentationCommentId.GetSymbolsForReferenceId(annotation.Data, compilation);
		}
	}
}
