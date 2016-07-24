using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using NHibernate.AsyncGenerator.Extensions;

namespace NHibernate.AsyncGenerator
{
	public class DocumentInfo : Dictionary<NamespaceDeclarationSyntax, NamespaceInfo>
	{
		public DocumentInfo(ProjectInfo projectInfo, Document document)
		{
			ProjectInfo = projectInfo;
			Document = document;
		}

		public async Task Initialize()
		{
			RootNode = (CompilationUnitSyntax) await Document.GetSyntaxRootAsync().ConfigureAwait(false);
			SemanticModel = await Document.GetSemanticModelAsync().ConfigureAwait(false);
			GlobalNamespaceInfo = new NamespaceInfo(this, SemanticModel.Compilation.GlobalNamespace, null);
		}

		public ProjectInfo ProjectInfo { get; }

		public Document Document { get; }

		public IReadOnlyList<string> Folders => Document.Folders;

		public string Name => Document.Name;

		public string Path => Document.FilePath;

		public CompilationUnitSyntax RootNode { get; private set; }

		public SemanticModel SemanticModel { get; private set; }

		public NamespaceInfo GlobalNamespaceInfo { get; private set; }

		public NamespaceInfo GetNamespaceInfo(ISymbol symbol, bool create = false)
		{
			var namespaceSymbol = symbol.ContainingNamespace;
			if (namespaceSymbol.IsGlobalNamespace)
			{
				return GlobalNamespaceInfo;
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
			return GetNamespaceInfo(node, namespaceSymbol, create);
		}

		public NamespaceInfo GetNamespaceInfo(SyntaxNode node, bool create = false)
		{
			if (node == null)
			{
				return GlobalNamespaceInfo;
			}
			var namespaceNode = node.AncestorsAndSelf().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
			if (namespaceNode == null)
			{
				return GlobalNamespaceInfo;
			}
			var namespaceSymbol =  SemanticModel.GetDeclaredSymbol(namespaceNode);
			return GetNamespaceInfo(namespaceNode, namespaceSymbol, create);
		}

		public NamespaceInfo GetNamespaceInfo(NamespaceDeclarationSyntax namespaceNode, INamespaceSymbol namespaceSymbol, bool create = false)
		{
			if (ContainsKey(namespaceNode))
			{
				return this[namespaceNode];
			}
			if (!create)
			{
				return null;
			}
			var docNamespace = new NamespaceInfo(this, namespaceSymbol, namespaceNode);
			Add(namespaceNode, docNamespace);
			if (Keys.GroupBy(o => o.Name).Any(o => o.Count() > 1)) //TODO REMOVE
			{

			}
			return docNamespace;
		}

		public MethodInfo GetOrCreateMethodInfo(IMethodSymbol symbol)
		{
			return GetNamespaceInfo(symbol, true).GetTypeInfo(symbol, true).GetMethodInfo(symbol, true);
		}

		public MethodInfo GetOrCreateMethodInfo(MethodDeclarationSyntax node)
		{
			return GetNamespaceInfo(node, true).GetTypeInfo(node, true).GetMethodInfo(node, true);
		}

		public MethodInfo GetMethodInfo(IMethodSymbol symbol)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol);
		}

		public TypeInfo GetTypeInfo(INamedTypeSymbol symbol)
		{
			return GetNamespaceInfo(symbol).GetTypeInfo(symbol);
		}

		public TypeInfo GetOrCreateTypeInfo(TypeDeclarationSyntax node)
		{
			return GetNamespaceInfo(node, true).GetTypeInfo(node, true);
		}

		public bool ContainsReference(IMethodSymbol symbol, ReferenceLocation reference)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol)?.References?.Contains(reference) == true;
		}
		/*
		public bool ContainsBodyToAsyncMethodReference(IMethodSymbol symbol, ReferenceLocation reference)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol)?.BodyToAsyncMethodsReferences?.Contains(reference) == true;
		}*/

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
