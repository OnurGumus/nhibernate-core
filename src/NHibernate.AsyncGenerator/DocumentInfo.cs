using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using NHibernate.AsyncGenerator.Extensions;

namespace NHibernate.AsyncGenerator
{
	public class DocumentInfo
	{
		public DocumentInfo(ProjectInfo projectInfo, Document document)
		{
			ProjectInfo = projectInfo;
			Document = document;
		}

		public ProjectInfo ProjectInfo { get; }

		public Document Document { get; }

		public IReadOnlyList<string> Folders => Document.Folders;

		public string Name => Document.Name;

		public string Path => Document.FilePath;

		public CompilationUnitSyntax RootNode { get; set; }

		public SemanticModel SemanticModel { get; set; }

		public Dictionary<INamespaceSymbol, NamespaceInfo> NamespaceInfos { get; } = new Dictionary<INamespaceSymbol, NamespaceInfo>();

		public NamespaceInfo GetNamespaceInfo(ISymbol symbol, bool create = false)
		{
			var namespaceSymbol = symbol.ContainingNamespace;
			if (NamespaceInfos.ContainsKey(namespaceSymbol))
			{
				return NamespaceInfos[namespaceSymbol];
			}
			if (!create)
			{
				return null;
			}

			var location = namespaceSymbol.Locations.Single(o => o.SourceTree.FilePath == Path);
			var node = RootNode.DescendantNodes()
							   .OfType<NamespaceDeclarationSyntax>()
							   .FirstOrDefault(
								   o =>
								   {
									   var identifier = o.ChildNodes().OfType<IdentifierNameSyntax>().SingleOrDefault();
									   if (identifier != null)
									   {
										   return identifier.Span == location.SourceSpan;
									   }
									   return o.ChildNodes().OfType<QualifiedNameSyntax>().Single().Right.Span == location.SourceSpan;
								   });
			if (node == null) // location.SourceSpan.Start == 0 -> a bug perhaps ???
			{
				node = RootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Single(o => o.FullSpan.End == location.SourceSpan.End);
			}
			var docNamespace = new NamespaceInfo(this, namespaceSymbol, node);
			NamespaceInfos.Add(namespaceSymbol, docNamespace);
			return docNamespace;
		}

		public MethodInfo GetOrCreateMethodInfo(IMethodSymbol symbol)
		{
			return GetNamespaceInfo(symbol, true).GetTypeInfo(symbol, true).GetMethodInfo(symbol, true);
		}

		public TypeInfo GetTypeInfo(INamedTypeSymbol symbol)
		{
			return GetNamespaceInfo(symbol).GetTypeInfo(symbol);
		}

		public bool ContainsReference(IMethodSymbol symbol, ReferenceLocation reference)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol)?.References?.Contains(reference) == true;
		}

		public bool ContainsBodyToAsyncMethodReference(IMethodSymbol symbol, ReferenceLocation reference)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol)?.BodyToAsyncMethodsReferences?.Contains(reference) == true;
		}

		public ISymbol GetEnclosingMethodOrPropertyOrField(ReferenceLocation reference)
		{
			var enclosingSymbol = SemanticModel.GetEnclosingSymbol(reference.Location.SourceSpan.Start);

			for (var current = enclosingSymbol; current != null; current = current.ContainingSymbol)
			{
				if (current.Kind == SymbolKind.Field)
				{
					return current;
				}

				if (current.Kind == SymbolKind.Property)
				{
					return current;
				}

				if (current.Kind == SymbolKind.Method)
				{
					var method = (IMethodSymbol)current;
					if (method.IsAccessor())
					{
						return method.AssociatedSymbol;
					}

					if (method.MethodKind != MethodKind.AnonymousFunction)
					{
						return method;
					}
				}
			}
			// reference to a cref
			return null;
		}
	}
}
