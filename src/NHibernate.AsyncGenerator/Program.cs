using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Shared.Extensions;
using NHibernate.AsyncGenerator;
using NHibernate.AsyncGenerator.Extensions;
using Nito.AsyncEx;
using DocumentInfo = NHibernate.AsyncGenerator.DocumentInfo;

namespace NHibernate.AsyncGenerator
{
	internal class DocumentAsyncMember
	{
		public DocumentAsyncMember(ISymbol symbol, MemberDeclarationSyntax node)
		{
			Symbol = symbol;
			Node = node;
			if (node is ClassDeclarationSyntax)
			{
				
			}
		}

		public ISymbol Symbol { get; }

		public MemberDeclarationSyntax Node { get; }

		public HashSet<ReferenceLocation> References { get; } = new HashSet<ReferenceLocation>();
	}

	internal class DocumentType
	{
		public DocumentType(INamedTypeSymbol symbol, TypeDeclarationSyntax node)
		{
			Symbol = symbol;
			Node = node;
		}

		public INamedTypeSymbol Symbol { get; }

		public TypeDeclarationSyntax Node { get; }

		public Dictionary<ISymbol, DocumentAsyncMember> AsyncMembers { get; } = new Dictionary<ISymbol, DocumentAsyncMember>();

		public Dictionary<INamedTypeSymbol, DocumentType> Types { get; } = new Dictionary<INamedTypeSymbol, DocumentType>();

		public DocumentAsyncMember GetAsyncMember(ISymbol symbol, bool create = false)
		{
			if (AsyncMembers.ContainsKey(symbol))
			{
				return AsyncMembers[symbol];
			}
			if (!create)
			{
				return null;
			}
			var location = symbol.Locations.Single(o => !o.SourceTree.FilePath.Contains(@"\Async\"));
			var memberNodes = Node.DescendantNodes()
									 .OfType<MemberDeclarationSyntax>()
									 .Where(o => o.Span.Contains(location.SourceSpan))
									 .ToList();
			MemberDeclarationSyntax memberNode;
			if (memberNodes.Count > 1)
			{
				memberNode = memberNodes.Last();
			}
			else
			{
				if (memberNodes.Count == 0)
				{
					
				}

				memberNode = memberNodes[0];
			}

			var asyncMember = new DocumentAsyncMember(symbol, memberNode);
			AsyncMembers.Add(symbol, asyncMember);
			return asyncMember;
		}

	}

	internal class DocumentNamespace
	{
		public DocumentNamespace(INamespaceSymbol symbol, NamespaceDeclarationSyntax node)
		{
			Symbol = symbol;
			Node = node;
		}

		public INamespaceSymbol Symbol { get; }

		public NamespaceDeclarationSyntax Node { get; }

		public Dictionary<INamedTypeSymbol, DocumentType> Types { get; } = new Dictionary<INamedTypeSymbol, DocumentType>();

		public DocumentType GetType(ISymbol symbol, bool create = false)
		{
			var nestedTypes = new Stack<INamedTypeSymbol>();
			var type = symbol.ContainingType;
			while (type != null)
			{
				nestedTypes.Push(type);
				type = type.ContainingType;
			}
			DocumentType currentDocType = null;
			var path = Node.SyntaxTree.FilePath;
			while (nestedTypes.Count > 0)
			{
				var typeSymbol = nestedTypes.Pop();
				if ((currentDocType?.Types ?? Types).ContainsKey(typeSymbol))
				{
					currentDocType = (currentDocType?.Types ?? Types)[typeSymbol];
					continue;
				}
				if (!create)
				{
					return null;
				}

				var location = typeSymbol.Locations.SingleOrDefault(o => o.SourceTree.FilePath == path);
				if (location == null)
				{
					
				}
				var node = Node.DescendantNodes()
							   .OfType<TypeDeclarationSyntax>()
							   .SingleOrDefault(o => o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken)).Span ==  location.SourceSpan);
				if (node == null)
				{
				}
				var docType = new DocumentType(typeSymbol, node);
				(currentDocType?.Types ?? Types).Add(typeSymbol, docType);
				currentDocType = docType;
			}
			return currentDocType;
		}
	}

	internal class DocumentInfo
	{
		public DocumentInfo(Document document)
		{
			Path = document.FilePath;
			Folders = document.Folders;
			Name = document.Name;
		}

		public IReadOnlyList<string> Folders { get; }

		public string Name { get; }

		public string Path { get; }

		public CompilationUnitSyntax RootNode { get; set; }

		public SemanticModel SemanticModel { get; set; }

		public Dictionary<INamespaceSymbol, DocumentNamespace> Namespaces { get; } = new Dictionary<INamespaceSymbol, DocumentNamespace>();

		public Dictionary<ReferenceLocation, ISymbol> ReferenceLocations { get; } = new Dictionary<ReferenceLocation, ISymbol>();

		public void AddAsyncMember(ISymbol memberSymbol)
		{
			GetOrCreateAsyncMember(memberSymbol);
		}

		public DocumentNamespace GetNamespace(ISymbol symbol, bool create = false) 
		{
			var namespaceSymbol = symbol.ContainingNamespace;
			if (Namespaces.ContainsKey(namespaceSymbol))
			{
				return Namespaces[namespaceSymbol];
			}
			if (!create)
			{
				return null;
			}

			var location = namespaceSymbol.Locations.Single(o => o.SourceTree.FilePath == Path);
			var node = RootNode.DescendantNodes()
							   .OfType<NamespaceDeclarationSyntax>()
							   .SingleOrDefault(
								   o =>
								   {
									   var identifier = o.ChildNodes().OfType<IdentifierNameSyntax>().SingleOrDefault();
									   if (identifier != null)
									   {
										   return identifier.Span == location.SourceSpan;
									   }
									   return o.ChildNodes().OfType<QualifiedNameSyntax>().Single().Right.Span == location.SourceSpan;
								   });
			if (node == null)
			{
				
			}
			var docNamespace = new DocumentNamespace(namespaceSymbol, node);
			Namespaces.Add(namespaceSymbol, docNamespace);
			return docNamespace;
		}

		public DocumentAsyncMember GetOrCreateAsyncMember(ISymbol symbol)
		{
			return GetNamespace(symbol, true).GetType(symbol, true).GetAsyncMember(symbol, true);
		}

		public bool ContainsReference(ISymbol symbol, ReferenceLocation reference)
		{
			return GetNamespace(symbol)?.GetType(symbol)?.GetAsyncMember(symbol)?.References?.Contains(reference) == true;
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

			// try harder!
			/*
			var typeSymbol = enclosingSymbol as ITypeSymbol;
			if (typeSymbol != null)
			{
				var sourceSpan = reference.Location.SourceSpan;
				var node = RootNode.DescendantNodes(descendIntoTrivia: true)
					.FirstOrDefault(o => o.Span == sourceSpan);
				if (node != null)
				{
					return new Tuple<ISymbol, bool>(SemanticModel.GetSymbolInfo(node).Symbol, true); // trivia
				}
				
			*/
			return null;
		}
	}

	internal class DocumentInfos : Dictionary<string, DocumentInfo>
	{
		public DocumentInfos(Project project)
		{
			Project = project;
		}

		public Project Project { get; }

		public async Task Analize()
		{
			foreach (var document in Project.Documents.Where(o => !o.FilePath.Contains(@"\Async\")))
			{
				await Add(document).ConfigureAwait(false);
			}
		}

		public bool Contains(Document document)
		{
			return ContainsKey(document.FilePath);
		}

		public async Task<DocumentInfo> GetOrCreate(Document document)
		{
			if (ContainsKey(document.FilePath))
			{
				return this[document.FilePath];
			}
			var info = new DocumentInfo(document)
			{
				RootNode = (CompilationUnitSyntax)await document.GetSyntaxRootAsync().ConfigureAwait(false),
				SemanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false)
			};
			Add(document.FilePath, info);
			return info;
		}

		public async Task<DocumentInfo> Add(Document document)
		{
			if (document.Project != Project)
			{
				throw new NotSupportedException("Multiple Project for DocumentInfos");
			}

			var info = await GetOrCreate(document).ConfigureAwait(false);

			foreach (var typeDeclaration in info.RootNode
												.DescendantNodes()
												.OfType<TypeDeclarationSyntax>())
			{
				var symbolInfo = info.SemanticModel.GetDeclaredSymbol(typeDeclaration);
				foreach (var memberSymbol in symbolInfo
					.GetMembers()
					.Where(o => o.GetAttributes().Any(a => a.AttributeClass.Name == "AsyncAttribute")))
				{
					info.AddAsyncMember(memberSymbol);
					await GetAllReferenceLocations(
						await SymbolFinder.FindReferencesAsync(memberSymbol, Project.Solution).ConfigureAwait(false)).ConfigureAwait(false);
				}
			}
			return info;
		}

		private HashSet<ReferenceLocation> IgnoredReferenceLocation { get; } = new HashSet<ReferenceLocation>();

		private async Task GetAllReferenceLocations(IEnumerable<ReferencedSymbol> references, int maxDepth = int.MaxValue, int depth = 0)
		{
			if (depth >= maxDepth)
			{
				return;
			}
			depth++;
			foreach (var refLocation in references.SelectMany(o => o.Locations).Where(o => !o.Location.SourceTree.FilePath.Contains(@"\Async\")))
			{
				if (IgnoredReferenceLocation.Contains(refLocation))
				{
					continue;
				}
				var info = await GetOrCreate(refLocation.Document).ConfigureAwait(false);
				var symbol = info.GetEnclosingMethodOrPropertyOrField(refLocation);
				if (symbol == null)
				{
					continue;
				}
				// check if we already processed the reference
				if (info.ContainsReference(symbol, refLocation))
				{
					continue;
				}

				// check if the symbol is a method otherwise skip
				var methodSymbol = symbol as IMethodSymbol;
				if (methodSymbol == null)
				{
					Console.WriteLine($"Symbol {symbol} is not a method and cannot be made async");
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}
				if (methodSymbol.MethodKind != MethodKind.Ordinary)
				{
					Console.WriteLine($"Method {methodSymbol} is a {methodSymbol.MethodKind} and cannot be made async");
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}

				var baseMethods = new HashSet<ISymbol>();
				if (methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
				{
					baseMethods.Add(methodSymbol);
				}

				// check if the method is implementing an external interface if the skip as we cannot modify externals
				var type = methodSymbol.ContainingType;
				var skip = false;
				foreach (var interfaceMember in type.AllInterfaces
					.SelectMany(o => o.GetMembers(methodSymbol.Name)
						.Where(m => type.FindImplementationForInterfaceMember(m) != null)))
				{
					var synatx = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
					if (synatx == null)
					{
						Console.WriteLine($"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
						IgnoredReferenceLocation.Add(refLocation);
						skip = true;
						break;
					}
					baseMethods.Add(interfaceMember);
				}
				if (skip)
				{
					continue;
				}

				// check if the method is overriding an external method
				var overridenMethod = methodSymbol.OverriddenMethod;
				while (overridenMethod != null)
				{
					var synatx = overridenMethod.DeclaringSyntaxReferences.SingleOrDefault();
					if (synatx == null)
					{
						Console.WriteLine($"Method {methodSymbol} overrides an external method {overridenMethod} and cannot be made async");
						IgnoredReferenceLocation.Add(refLocation);
						skip = true;
						break;
					}
					if (overridenMethod.OverriddenMethod != null)
					{
						overridenMethod = overridenMethod.OverriddenMethod;
					}
					else
					{
						break;
					}
				}
				if (skip)
				{
					continue;
				}

				// save the reference
				info.GetOrCreateAsyncMember(methodSymbol).References.Add(refLocation);

				// get and save all interface implementations
				foreach (var interfaceMethod in baseMethods)
				{
					var implementations = await SymbolFinder.FindImplementationsAsync(interfaceMethod, Project.Solution).ConfigureAwait(false);
					foreach (var implementation in implementations)
					{
						var synatx = implementation.DeclaringSyntaxReferences.Single();
						var doc = Project.GetDocument(synatx.SyntaxTree);
						info = await GetOrCreate(doc).ConfigureAwait(false);
						info.GetOrCreateAsyncMember(implementation);
					}
				}

				// get and save all derived methods
				if (overridenMethod != null)
				{
					var overrides = await SymbolFinder.FindOverridesAsync(overridenMethod, Project.Solution).ConfigureAwait(false);
					foreach (var overide in overrides)
					{
						var synatx = overide.DeclaringSyntaxReferences.Single();
						var doc = Project.GetDocument(synatx.SyntaxTree);
						info = await GetOrCreate(doc).ConfigureAwait(false);
						info.GetOrCreateAsyncMember(overide);
					}
					baseMethods.Add(overridenMethod);
				}

				if (!baseMethods.Any())
				{
					baseMethods.Add(methodSymbol);
				}

				foreach (var baseMethod in baseMethods)
				{
					await GetAllReferenceLocations(await SymbolFinder.FindReferencesAsync(baseMethod, Project.Solution).ConfigureAwait(false), maxDepth, depth)
							.ConfigureAwait(false);
				}

			}
		}

		
	}




	internal class DocumentWriter
	{
		public DocumentWriter(DocumentInfo info)
		{
			DocumentInfo = info;
			var path = "";
			for (var i = 0; i < DocumentInfo.Folders.Count; i++)
			{
				path += @"..\";
			}
			path += @"Async\";
			path += string.Join(@"\", DocumentInfo.Folders);
			DestinationFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(DocumentInfo.Path), path));
		}

		public DocumentInfo DocumentInfo { get; }

		public string DestinationFolder { get; }

		private MethodDeclarationSyntax RewiteMethod(DocumentAsyncMember asyncMember, DocumentType docType)
		{
			var memberNode = asyncMember.Node;
			var methodNode = memberNode as MethodDeclarationSyntax;
			if (methodNode == null)
			{
				Console.WriteLine($"Member:\r\n{memberNode}\r\ncannot be made async");
				//THROW
				return null;
			}

			foreach (var reference in asyncMember.References.OrderByDescending(o => o.Location.SourceSpan.Start))
			{
				if (methodNode.Body == null)
				{
					continue; // interface or abstract
				}
				SyntaxNode expressionNode;
				if (methodNode.Parent == null)
				{
					var startSpan = reference.Location.SourceSpan.Start - asyncMember.Node.Span.Start;
					var expressionNodes = methodNode.Body.DescendantNodes()
											   .Where(o => o.Span.Start == startSpan && o.Span.Length == reference.Location.SourceSpan.Length)
											   .ToList();
					expressionNode = expressionNodes.First();
				}
				else
				{
					expressionNode = methodNode.DescendantNodes()
											   .OfType<SimpleNameSyntax>()
											   .First(
												   o =>
												   {
													   if (o is GenericNameSyntax)
													   {
														   return o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken)).Span ==
																  reference.Location.SourceSpan;
													   }
													   return o.Span == reference.Location.SourceSpan;
												   });
				}

				var nameNode = expressionNode as SimpleNameSyntax;
				if (nameNode == null)
				{
					throw new Exception("Not an SimpleNameSyntax");
				}
				var anotation = Guid.NewGuid().ToString();
				methodNode = methodNode
					
					.ReplaceNode(nameNode,
					nameNode
						.WithIdentifier(SyntaxFactory.Identifier(nameNode.Identifier.Value + "Async"))
						.WithAdditionalAnnotations(new SyntaxAnnotation(anotation)));
				expressionNode = methodNode.GetAnnotatedNodes(anotation).Single();
				var invocationNode = expressionNode.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault();
				if (invocationNode == null)
				{
					Console.WriteLine($"InvocationExpressionSyntax not found for:\r\n{expressionNode}\r\n");
					//THROW
					continue;
				}
				methodNode = invocationNode.AddAwait(methodNode).WithoutTrivia();
			}
			methodNode = methodNode
				.WithAttributeLists(
					SyntaxFactory.List(methodNode.AttributeLists.Where(o => o.Attributes.All(a => a.Name.ToString() != "Async")))) // this removes also comments
				.ReturnAsTask((IMethodSymbol)asyncMember.Symbol)
				.WithIdentifier(SyntaxFactory.Identifier(methodNode.Identifier.Value + "Async"));

			if (docType.Node is ClassDeclarationSyntax && !methodNode.Modifiers.Any(SyntaxKind.AbstractKeyword))
			{
				methodNode = methodNode.AddAsync(methodNode);
			}
			return methodNode;
		}

		public SyntaxAnnotation AsyncMethodAnnotation = new SyntaxAnnotation("AsyncMethod");

		public void Write()
		{
			var rootNode = DocumentInfo.RootNode;
			var namespaceNodes = new List<MemberDeclarationSyntax>();
			foreach (var docNamespace in DocumentInfo.Namespaces.Values)
			{
				var namespaceNode = docNamespace.Node;
				var typeNodes = new List<MemberDeclarationSyntax>();
				foreach (var docType in docNamespace.Types.Values)
				{
					var typeNode = docType.Node;
					var memberNodes = new List<MemberDeclarationSyntax>();
					foreach (var asyncMember in docType.AsyncMembers.Values)
					{
						memberNodes.Add(RewiteMethod(asyncMember, docType));
					}
					// TODO: inifinite levels of nested types
					foreach (var subDocType in docType.Types.Values)
					{
						var subTypeNode = subDocType.Node;
						var subMemberNodes = new List<MemberDeclarationSyntax>();
						foreach (var asyncMember in subDocType.AsyncMembers.Values)
						{
							subMemberNodes.Add(RewiteMethod(asyncMember, subDocType));
						}

						subTypeNode = subTypeNode
							.AddPartial()
							.WithAttributes(SyntaxFactory.List(new List<AttributeListSyntax>
							{
								SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(
									new List<AttributeSyntax>
									{
										SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("System.CodeDom.Compiler.GeneratedCode(\"AsyncGenerator\", \"1.0.0\")"))
									}
								))
							}))
							.WithMembers(SyntaxFactory.List(subMemberNodes));
						memberNodes.Add(subTypeNode);
					}

					// add partial to the original file
					//var partialTypeDeclaration = typeDeclaration.AddPartial();
					//if (partialTypeDeclaration != typeDeclaration)
					//{
					//	partialTypeDeclaration = partialTypeDeclaration.NormalizeWhitespace("	");
					//	rootChanged = true;
					var modifier = typeNode.GetModifierWithLeadingTrivia();
					if (modifier.HasValue)
					{
						typeNode = typeNode.ReplaceToken(modifier.Value, modifier.Value.WithLeadingTrivia(SyntaxTriviaList.Empty));
					}
					typeNode = typeNode
						.AddPartial()
						.WithAttributes(SyntaxFactory.List(new List<AttributeListSyntax>
						{
							SyntaxFactory.AttributeList(
								SyntaxFactory.Token(SyntaxKind.OpenBracketToken).WithLeadingTrivia(modifier?.LeadingTrivia ?? SyntaxTriviaList.Empty),
								null,
								SyntaxFactory.SeparatedList(
									new List<AttributeSyntax>
									{
										SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("System.CodeDom.Compiler.GeneratedCode(\"AsyncGenerator\", \"1.0.0\")"))
									}
								),
								SyntaxFactory.Token(SyntaxKind.CloseBracketToken)
							)
						}))
						.WithMembers(SyntaxFactory.List(memberNodes.Where(o => o != null) /*TODO: STRICT*/ ));
					typeNode = typeNode.RemoveNodes(
							typeNode.DescendantNodes(descendIntoTrivia: true).OfType<DirectiveTriviaSyntax>(), SyntaxRemoveOptions.KeepNoTrivia); // remove invalid #endregion
					typeNodes.Add(typeNode);
				}
				namespaceNodes.Add(namespaceNode
						.WithMembers(SyntaxFactory.List(typeNodes)));
			}
			var content = rootNode
				.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")))
				.WithMembers(SyntaxFactory.List(namespaceNodes))
				.NormalizeWhitespace("	")
				.ToFullString();

			if (!Directory.Exists(DestinationFolder))
			{
				Directory.CreateDirectory(DestinationFolder);
			}
			File.WriteAllText($"{DestinationFolder}\\{DocumentInfo.Name}", content);
		}
	}




	class Program
	{
		static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var nhPath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\NHibernate\"));


			var workspace = MSBuildWorkspace.Create();
			var project = workspace.OpenProjectAsync(Path.Combine(nhPath, "NHibernate.csproj")).Result;
			var documentInfos = new DocumentInfos(project);
			AsyncContext.Run(() => documentInfos.Analize());

			foreach (var pair in documentInfos.Where(o => o.Value.Namespaces.Any()))
			{
				var writer = new DocumentWriter(pair.Value);
				writer.Write();
			}
		}
	}
}
