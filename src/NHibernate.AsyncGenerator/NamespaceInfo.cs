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
	public class NamespaceInfo : Dictionary<TypeDeclarationSyntax, TypeInfo>
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

		//public Dictionary<TypeDeclarationSyntax, TypeInfo> TypeInfos { get; } = new Dictionary<TypeDeclarationSyntax, TypeInfo>();

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
				var location = typeSymbol.Locations.Single(o => o.SourceTree.FilePath == path);
				var node = Node.DescendantNodes()
							   .OfType<TypeDeclarationSyntax>()
							   .First(o => o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);

				if ((currentDocType?.TypeInfos ?? this).ContainsKey(node))
				{
					currentDocType = (currentDocType?.TypeInfos ?? this)[node];
					continue;
				}
				if (!create)
				{
					return null;
				}

				var docType = new TypeInfo(this, typeSymbol, node);
				(currentDocType?.TypeInfos ?? this).Add(node, docType);
				if ((currentDocType?.TypeInfos ?? this).Keys.GroupBy(o => o.Identifier.ValueText).Any(o => o.Count() > 1))
				{

				}
				currentDocType = docType;
			}
			return currentDocType;
		}
	}
}
