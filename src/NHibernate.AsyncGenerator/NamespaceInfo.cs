using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

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

		/// <summary>
		/// Type references that need to be renamed
		/// </summary>
		public HashSet<ReferenceLocation> TypeReferences { get; } = new HashSet<ReferenceLocation>();

		public TypeInfo GetTypeInfo(TypeDeclarationSyntax node, bool create = false)
		{
			var typeSymbol = DocumentInfo.SemanticModel.GetDeclaredSymbol(node);
			return GetTypeInfo(typeSymbol, create);
		}

		public TypeInfo GetTypeInfo(MethodDeclarationSyntax node, bool create = false)
		{
			var typeSymbol = DocumentInfo.SemanticModel.GetDeclaredSymbol(node).ContainingType;
			return GetTypeInfo(typeSymbol, create);
		}

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
			var path = DocumentInfo.Path;
			while (nestedTypes.Count > 0)
			{
				var typeSymbol = nestedTypes.Pop().OriginalDefinition;
				var location = typeSymbol.Locations.Single(o => o.SourceTree.FilePath == path);
				var namespaceNode = Node ?? (SyntaxNode) DocumentInfo.RootNode; // global namespace
				var node = namespaceNode.DescendantNodes()
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

				var docType = new TypeInfo(this, currentDocType, typeSymbol, node);
				(currentDocType?.TypeInfos ?? this).Add(node, docType);
				if ((currentDocType?.TypeInfos ?? this).Keys.GroupBy(o => 
					o.Identifier.ValueText
					+ (o.TypeParameterList != null
					? $"<{string.Join(",", o.TypeParameterList.Parameters.Select(p => p.Identifier.ValueText))}>"
					: "")).Any(o => o.Count() > 1)) //TODO REMOVE
				{
					throw new Exception("dasdas");
				}
				currentDocType = docType;
			}
			return currentDocType;
		}
	}
}
