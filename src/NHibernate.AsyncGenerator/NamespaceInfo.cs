using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public class NamespaceInfo
	{
		public NamespaceInfo(DocumentInfo documentInfo, INamespaceSymbol symbol, NamespaceDeclarationSyntax node)
		{
			DocumentInfo = documentInfo;
			Symbol = symbol;
			Node = node;
		}

		public DocumentInfo DocumentInfo { get; }

		public INamespaceSymbol Symbol { get; }

		public NamespaceDeclarationSyntax Node { get; }

		public Dictionary<INamedTypeSymbol, TypeInfo> TypeInfos { get; } = new Dictionary<INamedTypeSymbol, TypeInfo>();

		public TypeInfo GetTypeInfo(IMethodSymbol symbol, bool create = false)
		{
			return GetTypeInfo(symbol.ContainingType, create);
		}

		public TypeInfo GetTypeInfo(INamedTypeSymbol type, bool create = false)
		{
			var nestedTypes = new Stack<INamedTypeSymbol>();
			while (type != null)
			{
				nestedTypes.Push(type);
				type = type.ContainingType;
			}
			TypeInfo currentDocType = null;
			var path = Node.SyntaxTree.FilePath;
			while (nestedTypes.Count > 0)
			{
				var typeSymbol = nestedTypes.Pop().OriginalDefinition;

				if ((currentDocType?.TypeInfos ?? TypeInfos).ContainsKey(typeSymbol))
				{
					currentDocType = (currentDocType?.TypeInfos ?? TypeInfos)[typeSymbol];
					continue;
				}
				if (!create)
				{
					return null;
				}

				var location = typeSymbol.Locations.Single(o => o.SourceTree.FilePath == path);
				var node = Node.DescendantNodes()
							   .OfType<TypeDeclarationSyntax>()
							   .Single(o => o.ChildTokens().Single(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);
				var docType = new TypeInfo(this, typeSymbol, node);
				(currentDocType?.TypeInfos ?? TypeInfos).Add(typeSymbol, docType);
				currentDocType = docType;
			}
			return currentDocType;
		}
	}
}
