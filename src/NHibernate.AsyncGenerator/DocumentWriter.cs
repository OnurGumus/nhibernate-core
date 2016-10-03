using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using NHibernate.AsyncGenerator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NHibernate.AsyncGenerator
{
	public class DocumentWriter
	{
		public DocumentWriter(DocumentInfo info, WriterConfiguration configuration)
		{
			DocumentInfo = info;
			Configuration = configuration;
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

		public WriterConfiguration Configuration { get; }

		public string DestinationFolder { get; }

		private FieldDeclarationSyntax GetAsyncLockField()
		{
			var lockConfig = Configuration.Async.Lock;
			return FieldDeclaration(
				VariableDeclaration(
					IdentifierName(lockConfig.TypeName)
				)
				.WithVariables(
					SingletonSeparatedList(
						VariableDeclarator(
							Identifier(lockConfig.FieldName)
						)
						.WithInitializer(
							EqualsValueClause(
								ObjectCreationExpression(
									IdentifierName(lockConfig.TypeName)
								)
								.WithArgumentList(
									ArgumentList()
								)
							)
						)
					)
				)
			)
			.WithModifiers(
				TokenList(
					Token(SyntaxKind.PrivateKeyword),
					Token(SyntaxKind.ReadOnlyKeyword)
				)
			);
		}

		private BlockSyntax AsyncLockBlock(BlockSyntax block)
		{
			var lockConfig = Configuration.Async.Lock;
			return Block(
				SingletonList<StatementSyntax>(
					UsingStatement(block)
					.WithDeclaration(
						VariableDeclaration(
							IdentifierName("var")
						)
						.WithVariables(
							SingletonSeparatedList(
								VariableDeclarator(Identifier("releaser"))
									.WithInitializer(EqualsValueClause(
										AwaitExpression(
											InvocationExpression(
												MemberAccessExpression(
													SyntaxKind.SimpleMemberAccessExpression,
													IdentifierName(lockConfig.FieldName),
													IdentifierName(lockConfig.MethodName)
												)
											)
										)
									)
								)
							)
						)
					)
				)
			);
		}

		private MethodDeclarationSyntax RewiteMethod(MethodInfo methodInfo)
		{
			// TODO: if a method cannot be converted to async we should not convert dependencies too
			if (!methodInfo.CanBeAsnyc && !methodInfo.RelatedMethods.Any() && !methodInfo.Missing)
			{
				if (methodInfo.TypeInfo.TypeTransformation == TypeTransformation.Partial)
				{
					return null;
				}
				return methodInfo.Node;
			}

			var taskConflict = !DocumentInfo.ProjectInfo.IsNameUniqueInsideNamespace(methodInfo.TypeInfo.NamespaceInfo.Symbol, "Task");
			if (!methodInfo.HasBody)
			{
				return RemoveLeadingRegions(
					methodInfo.Node
							  .WithoutAttribute("Async")
							  .ReturnAsTask(methodInfo.Symbol, taskConflict)
							  .WithIdentifier(Identifier(methodInfo.Node.Identifier.Value + "Async")));
			}

			var methodNode = methodInfo.Node.WithoutTrivia(); // references have spans without trivia

			// we first need to annotate nodes that will be modified in order to find them later on. We cannot rely on spans after the first modification as they will change
			var typeReferencesAnnotations = new List<string>();
			foreach (var reference in methodInfo.TypeReferences.OrderByDescending(o => o.Location.SourceSpan.Start))
			{
				var startSpan = reference.Location.SourceSpan.Start - methodInfo.Node.Span.Start;
				var nameNode = methodNode.DescendantNodes()
										 .OfType<SimpleNameSyntax>()
										 .First(
											 o =>
											 {
												 if (!o.IsKind(SyntaxKind.GenericName))
												 {
													 return o.Span.Start == startSpan && o.Span.Length == reference.Location.SourceSpan.Length;
												 }
												 var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
												 return token.Span.Start == startSpan && token.Span.Length == reference.Location.SourceSpan.Length;
											 });
				var annotation = Guid.NewGuid().ToString();
				methodNode = methodNode.ReplaceNode(nameNode, nameNode.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));
				typeReferencesAnnotations.Add(annotation);
			}

			var referenceAnnotations = new Dictionary<string, MethodReferenceResult>();
			foreach (var referenceResult in methodInfo.ReferenceResults
														.Where(o => !o.Ignore))
			{
				var reference = referenceResult.ReferenceLocation;
				var startSpan = reference.Location.SourceSpan.Start - methodInfo.Node.Span.Start;
				var nameNode = methodNode.Body.DescendantNodes()
											.OfType<SimpleNameSyntax>()
											.First(
												o =>
												{
													if (!o.IsKind(SyntaxKind.GenericName))
													{
														return o.Span.Start == startSpan && o.Span.Length == reference.Location.SourceSpan.Length;
													}
													var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
													return token.Span.Start == startSpan && token.Span.Length == reference.Location.SourceSpan.Length;
												});
				var annotation = Guid.NewGuid().ToString();
				methodNode = methodNode.ReplaceNode(nameNode, nameNode.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));
				referenceAnnotations.Add(annotation, referenceResult);
			}
			
			

			// modify references
			foreach (var refAnnotation in typeReferencesAnnotations)
			{
				var nameNode = methodNode.GetAnnotatedNodes(refAnnotation).OfType<SimpleNameSyntax>().First();
				methodNode = methodNode
							.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")));
			}

			foreach (var pair in referenceAnnotations)
			{
				var nameNode = methodNode.GetAnnotatedNodes(pair.Key).OfType<SimpleNameSyntax>().First();
				var referenceResult = pair.Value;
				if (!referenceResult.CanBeAwaited)
				{
					// modify only the method name
					methodNode = methodNode
						.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")));
					continue;
				}

				if (methodInfo.CanSkipAsync)
				{
					// return task instead of awaiting
					if (referenceResult.UsedAsReturnValue)
					{
						// modify only the method name
						methodNode = methodNode
							.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")));
					}
					else
					{
						// wrap into a return statement
						var expressionStatementNode = nameNode.Ancestors().OfType<ExpressionStatementSyntax>().First();
						methodNode = methodNode
							.ReplaceNode(
								expressionStatementNode,
								ReturnStatement(
									expressionStatementNode.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async"))).Expression
								)
							);
					}
				}
				else
				{
					if (referenceResult.PassedAsArgument)
					{
						if (referenceResult.WrapInsideAsyncFunction)
						{
							var argumentNode = nameNode.Ancestors().OfType<ArgumentSyntax>().First();
							var expressionNode = argumentNode.ChildNodes().OfType<ExpressionSyntax>().First();
							var lambdaNode = ParenthesizedLambdaExpression(
								AwaitExpression(
									InvocationExpression(expressionNode.ReplaceNode(nameNode,
										nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async"))))))
								.WithAsyncKeyword(
									Token(SyntaxKind.AsyncKeyword));
							methodNode = methodNode
								.ReplaceNode(expressionNode, lambdaNode);
						}
						continue;
					}

					var lastAwaitAnnotation = Guid.NewGuid().ToString();
					// await method
					var invocationNode = nameNode.Ancestors().OfType<InvocationExpressionSyntax>().First();
					var annotation = new SyntaxAnnotation(Guid.NewGuid().ToString());
					methodNode = methodNode
						.ReplaceNode(
							invocationNode,
							invocationNode
								.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")))
								.WithAdditionalAnnotations(annotation)
						);
					invocationNode = methodNode.GetAnnotatedNodes(annotation).OfType<InvocationExpressionSyntax>().Single();
					var awaitNode = invocationNode.AddAwait().WithAdditionalAnnotations(new SyntaxAnnotation(lastAwaitAnnotation));
					methodNode = methodNode.ReplaceNode(invocationNode, awaitNode);
					awaitNode = methodNode.GetAnnotatedNodes(lastAwaitAnnotation).Single();

					if (!referenceResult.MakeAnonymousFunctionAsync) continue;
					var functionNode = awaitNode.Ancestors().OfType<AnonymousFunctionExpressionSyntax>().First();
					if (functionNode.AsyncKeyword.Kind() == SyntaxKind.AsyncKeyword) continue;
					methodNode = methodNode.ReplaceNode(functionNode, functionNode.AddAsync());
				}
			}
			
			// do not create a async method if there is no async calls inside or is not a missing member
			if (!methodInfo.HasReferences && !methodInfo.Missing)
			{
				return methodNode
					.WithLeadingTrivia(methodInfo.Node.GetLeadingTrivia())
					.WithTrailingTrivia(methodInfo.Node.GetTrailingTrivia());
			}

			if (methodInfo.TypeInfo.ParentTypeInfo?.TypeTransformation != TypeTransformation.NewType && 
				methodInfo.TypeInfo.TypeTransformation != TypeTransformation.NewType &&
				!methodInfo.CanBeAsnyc && !methodInfo.IsEmpty) // forward call
			{
				var name = methodInfo.Node.TypeParameterList != null
					? GenericName(methodInfo.Node.Identifier.ValueText)
						.WithTypeArgumentList(
							TypeArgumentList(
								SeparatedList<TypeSyntax>(
									methodInfo.Node.TypeParameterList.Parameters.Select(o => IdentifierName(o.Identifier.ValueText))
									)))
					: (SimpleNameSyntax)IdentifierName(methodInfo.Node.Identifier.ValueText);

				var invocation = InvocationExpression(name)
					.WithArgumentList(
						ArgumentList(
							SeparatedList(
								methodInfo.Node.ParameterList.Parameters
										  .Select(o => Argument(IdentifierName(o.Identifier.ValueText)))
								)));
				if (methodInfo.Symbol.ReturnsVoid)
				{
					methodNode = methodNode.WithBody(Block(ExpressionStatement(invocation)));
				}
				else
				{
					methodNode = methodNode.WithBody(Block(ReturnStatement(invocation)));
				}
			}
			else if (methodInfo.HasYields)
			{
				var yieldRewriter = new YieldToAsyncMethodRewriter();
				methodNode = (MethodDeclarationSyntax)yieldRewriter.VisitMethodDeclaration(methodNode);
			}

			if (!methodInfo.CanBeAsnyc || methodInfo.CanSkipAsync || methodInfo.HasYields)
			{
				var taskRewriter = new ReturnTaskMethodRewriter(Configuration.Async.CustomTaskType);
				methodNode = (MethodDeclarationSyntax)taskRewriter.VisitMethodDeclaration(methodNode);
			}

			// check if method must run synhronized
			if (methodInfo.MustRunSynchronized)
			{
				methodNode = methodNode
					.WithBody(AsyncLockBlock(methodNode.Body))
					.WithoutAttribute("MethodImpl");
			}
			methodNode = methodNode
				.WithoutAttribute("Async")
				.ReturnAsTask(methodInfo.Symbol, taskConflict)
				.WithIdentifier(Identifier(methodNode.Identifier.Value + "Async"));

			if (methodInfo.MustRunSynchronized || (!methodInfo.CanSkipAsync && !methodInfo.HasYields && methodInfo.CanBeAsnyc))
			{
				methodNode = methodNode.AddAsync(methodNode);
			}

			return RemoveLeadingRegions(methodNode.WithLeadingTrivia(methodInfo.Node.GetLeadingTrivia()))
				.WithTrailingTrivia(methodInfo.Node.GetTrailingTrivia());
		}

		private SyntaxTriviaList RemoveRegions(SyntaxTriviaList list) 
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

		private T RemoveLeadingRegions<T>(T memberNode) where T : MemberDeclarationSyntax
		{
			// remove all regions as not all methods will be written in the type
			var leadingTrivia = memberNode.GetLeadingTrivia();
			memberNode = memberNode.WithLeadingTrivia(RemoveRegions(leadingTrivia));

			var baseTypeNode = memberNode as BaseTypeDeclarationSyntax;
			if (baseTypeNode == null)
			{
				return memberNode;
			}
			return memberNode
				.ReplaceToken(
					baseTypeNode.CloseBraceToken,
					baseTypeNode.CloseBraceToken
								.WithLeadingTrivia(RemoveRegions(baseTypeNode.CloseBraceToken.LeadingTrivia)));
		}

		private class RewrittenNode
		{
			public SyntaxNode Original { get; set; }

			public SyntaxNode Rewritten { get; set; }

			public string Annotation { get; set; }

			public MethodInfo MethodInfo { get; set; }
		}

		private class TypeInfoMetadata
		{
			public string NodeAnnotation { get; set; }

			public List<RewrittenNode> RewrittenNodes { get; } = new List<RewrittenNode>();

			public bool MissingPartialKeyword { get; set; }

			public bool TaskUsed { get; set; }

			public bool AsyncLockUsed { get; set; }
		}

		private class RewriteTypeResult
		{
			public bool ReplaceOriginalNode { get; set; }

			public TypeDeclarationSyntax OriginalNode { get; set; }

			public TypeDeclarationSyntax Node { get; set; }

			public bool TaskUsed { get; set; }

			public bool AsyncLockUsed { get; set; }
		}

		public class TransformDocumentResult
		{
			public CompilationUnitSyntax NewRootNode { get; set; }

			public CompilationUnitSyntax OriginalRootNode { get; set; }
		}

		private RewriteTypeResult RewriteType(TypeInfo rootTypeInfo, bool onlyMissingMembers = false)
		{
			var result = new RewriteTypeResult();
			var rootTypeNode = rootTypeInfo.Node;
			var startRootTypeSpan = rootTypeInfo.Node.SpanStart;

			rootTypeNode = rootTypeNode.WithoutTrivia(); // references have spans without trivia

			// we first need to annotate nodes that will be modified in order to find them later on. We cannot rely on spans after the first modification as they will change
			var typeInfoMetadatas = new Dictionary<TypeInfo, TypeInfoMetadata>();
			foreach (var typeInfo in rootTypeInfo.GetDescendantTypeInfosAndSelf()
				.Where(o => o.TypeTransformation != TypeTransformation.None)
				.OrderByDescending(o => o.Node.SpanStart))
			{
				var annotation = Guid.NewGuid().ToString();
				var metadata = new TypeInfoMetadata
				{
					NodeAnnotation = annotation,
					MissingPartialKeyword = typeInfo.TypeTransformation == TypeTransformation.Partial && !typeInfo.Node.IsPartial()
				};

				var typeSpanStart = typeInfo.Node.SpanStart - startRootTypeSpan;
				var typeSpanLength = typeInfo.Node.Span.Length;
				var node = rootTypeNode.DescendantNodesAndSelf().OfType<TypeDeclarationSyntax>()
					.First(o => o.SpanStart == typeSpanStart && o.Span.Length == typeSpanLength);
				rootTypeNode = rootTypeNode.ReplaceNode(node, node.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));

				// process references
				// TODO: typereferences can be changed only if the type is copied, need to find all dependencies an create new types for them
				if (!onlyMissingMembers && rootTypeInfo.TypeTransformation == TypeTransformation.NewType)
				{
					foreach (var reference in typeInfo.TypeReferences)
					{
						var refSpanStart = reference.Location.SourceSpan.Start - startRootTypeSpan;
						var refSpanLength = reference.Location.SourceSpan.Length;
						if (refSpanStart < 0)
						{
							// cref
							//var startSpan = reference.Location.SourceSpan.Start - rootTypeInfo.Node.GetLeadingTrivia().Span.Start;
							//var crefNode = leadingTrivia.First(o => o.SpanStart == startSpan && o.Span.Length == refSpanLength);
							continue;
						}

						var nameNode = rootTypeNode.DescendantNodes()
												 .OfType<SimpleNameSyntax>()
												 .First(
													 o =>
													 {
														 if (!o.IsKind(SyntaxKind.GenericName))
														 {
															 return o.Span.Start == refSpanStart && o.Span.Length == refSpanLength;
														 }
														 var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
														 return token.Span.Start == refSpanStart && token.Span.Length == refSpanLength;
													 });
						annotation = Guid.NewGuid().ToString();
						rootTypeNode = rootTypeNode.ReplaceNode(nameNode, nameNode.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));

						metadata.RewrittenNodes.Add(new RewrittenNode
						{
							Annotation = annotation,
							Original = nameNode,
							Rewritten = nameNode.WithIdentifier(Identifier(nameNode.Identifier.ValueText + "Async"))
						});
					}
				}
				

				// process methods
				foreach (var methodInfo in typeInfo.MethodInfos.Values)
				{
					var methodSpanStart = methodInfo.Node.SpanStart - startRootTypeSpan;
					var methodSpanLength = methodInfo.Node.Span.Length;
					var methodNode = rootTypeNode.DescendantNodes()
											 .OfType<MethodDeclarationSyntax>()
											 .First(o => o.SpanStart == methodSpanStart && o.Span.Length == methodSpanLength);
					annotation = Guid.NewGuid().ToString();
					rootTypeNode = rootTypeNode.ReplaceNode(methodNode, methodNode.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));

					var removeNode = methodInfo.Ignore;

					metadata.TaskUsed |= methodInfo.CanSkipAsync || methodInfo.IsEmpty || methodInfo.HasYields || !methodInfo.CanBeAsnyc;
					result.TaskUsed |= metadata.TaskUsed;
					metadata.AsyncLockUsed |= methodInfo.MustRunSynchronized;
					result.AsyncLockUsed |= metadata.AsyncLockUsed;
					metadata.RewrittenNodes.Add(new RewrittenNode
					{
						Original = methodNode,
						Rewritten = removeNode ? null : RewiteMethod(methodInfo),
						MethodInfo = methodInfo,
						Annotation = annotation
					});
				}

				typeInfoMetadatas.Add(typeInfo, metadata);
			}

			result.OriginalNode = rootTypeNode.WithTriviaFrom(rootTypeInfo.Node);

			foreach (var typeInfo in rootTypeInfo.GetDescendantTypeInfosAndSelf()
				.Where(o => o.TypeTransformation != TypeTransformation.None)
				.OrderByDescending(o => o.Node.SpanStart))
			{
				var metadata = typeInfoMetadatas[typeInfo];
				// add partial to the original node
				if (metadata.MissingPartialKeyword)
				{
					result.ReplaceOriginalNode = true;
					var typeNode = result.OriginalNode.GetAnnotatedNodes(metadata.NodeAnnotation).OfType<TypeDeclarationSyntax>().First();
					result.OriginalNode = result.OriginalNode.ReplaceNode(typeNode, typeNode
						.AddPartial(t => t.WithTrailingTrivia(SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "))));
				}

				// if the root type has to be a new type then all nested types have to be new types
				if (!onlyMissingMembers && rootTypeInfo.TypeTransformation == TypeTransformation.NewType)
				{
					// replace all rewritten nodes
					foreach (var rewNode in metadata.RewrittenNodes
						.Where(o => o.MethodInfo == null || (!o.MethodInfo.Missing /*&& !o.MethodInfo.HasRequiredExternalMethods()*/))
						.OrderByDescending(o => o.Original.SpanStart))
					{
						var node = rootTypeNode.GetAnnotatedNodes(rewNode.Annotation).First();
						if (rewNode.Rewritten == null)
						{
							rootTypeNode = rootTypeNode.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia);
						}
						else
						{
							rootTypeNode = rootTypeNode.ReplaceNode(node, rewNode.Rewritten);
						}
					}
					// add missing members
					var typeNode = rootTypeNode.GetAnnotatedNodes(metadata.NodeAnnotation).OfType<TypeDeclarationSyntax>().First();

					rootTypeNode = rootTypeNode.ReplaceNode(typeNode, typeNode.WithMembers(
							typeNode.Members.Select(RemoveLeadingRegions).ToSyntaxList()
							.AddRange(
								metadata.RewrittenNodes
									.OrderBy(o => o.Original.SpanStart)
									.Where(o => o.MethodInfo != null && o.MethodInfo.Missing)
									.Select(o => o.Rewritten)
									.OfType<MethodDeclarationSyntax>())));

					typeNode = rootTypeNode.GetAnnotatedNodes(metadata.NodeAnnotation).OfType<TypeDeclarationSyntax>().First();
					var identifierToken = typeNode.ChildTokens().First(o => o.IsKind(SyntaxKind.IdentifierToken));
					rootTypeNode = rootTypeNode
						.ReplaceNode(
							typeNode,
							(typeInfo.TypeTransformation == TypeTransformation.NewType
								? typeNode.ReplaceToken(identifierToken, Identifier(identifierToken.ValueText + "Async"))
								: typeNode)
								.AddGeneratedCodeAttribute());

					// rename all constructors
					if (typeInfo.TypeTransformation == TypeTransformation.NewType)
					{
						while (true)
						{
							var ctorNode = rootTypeNode.DescendantNodes()
											   .OfType<ConstructorDeclarationSyntax>()
											   .FirstOrDefault(o => o.Identifier.ValueText == typeInfo.Symbol.Name);
							if (ctorNode == null)
							{
								break;
							}
							rootTypeNode = rootTypeNode.ReplaceNode(ctorNode, ctorNode.WithIdentifier(Identifier(typeInfo.Symbol.Name + "Async")));
						}
					}
				}
				else
				{
					var typeNode = rootTypeNode.GetAnnotatedNodes(metadata.NodeAnnotation).OfType<TypeDeclarationSyntax>().First();
					var newNodes = (onlyMissingMembers
						? metadata.RewrittenNodes
							.Where(o => o.Rewritten != null)
							.Where(o => o.MethodInfo != null && o.MethodInfo.Missing)
							.OrderBy(o => o.Original.SpanStart)
							.Select(o => o.Rewritten)
						: metadata.RewrittenNodes
							.Where(o => o.Rewritten != null)
							.Where(o => o.MethodInfo == null || (o.MethodInfo != null && (!o.MethodInfo.HasRequiredExternalMethods() || o.MethodInfo.Missing))) // for partials we wont have onlyMissingMembers = true 
							.OrderBy(o => o.Original.SpanStart)
							.Select(o => o.Rewritten))
						.Union(typeNode.DescendantNodes().OfType<TypeDeclarationSyntax>())
						.ToList();

					if (!newNodes.Any())
					{
						rootTypeNode = rootTypeNode.RemoveNode(typeNode, SyntaxRemoveOptions.KeepNoTrivia);
					}
					else
					{
						var newTypeNode = typeNode
						.AddPartial()
						.WithMembers(List(newNodes));
						newTypeNode = typeInfo.Symbol.Locations
										   .Where(o => DocumentInfo.ProjectInfo.ContainsKey(o.SourceTree.FilePath))
										   .Select(o => DocumentInfo.ProjectInfo[o.SourceTree.FilePath].GetTypeInfo(typeInfo.Symbol).Node)
										   .Any(n => n.AttributeLists.Any(o => o.Attributes.Any(a => a.Name.ToString().EndsWith("GeneratedCode"))))
							? newTypeNode.WithoutAttributes()
							: newTypeNode.AddGeneratedCodeAttribute(false);
						/*
						newTypeNode = newTypeNode.RemoveNodes(
								newTypeNode.DescendantNodes(descendIntoTrivia: true).OfType<DirectiveTriviaSyntax>(), SyntaxRemoveOptions.KeepNoTrivia); // remove invalid #endregion
						*/
						rootTypeNode = rootTypeNode
							.ReplaceNode(typeNode, RemoveLeadingRegions(newTypeNode));
					}
				}

				if (metadata.AsyncLockUsed)
				{
					var typeNode = rootTypeNode.GetAnnotatedNodes(metadata.NodeAnnotation).OfType<TypeDeclarationSyntax>().First();
					rootTypeNode = rootTypeNode
						.ReplaceNode(typeNode, typeNode
							.WithMembers(typeNode.Members.Insert(0, GetAsyncLockField())));
				}
			}

			// remove all regions as not all methods will be written in the type
			result.Node = rootTypeNode != null 
				? RemoveLeadingRegions(rootTypeNode)
				: null;
				/*.WithTriviaFrom(rootTypeInfo.Node)*/;
			return result;
		}

		public TransformDocumentResult Transform()
		{
			var result = new TransformDocumentResult();
			var rootNode = DocumentInfo.RootNode.WithoutTrivia();
			var namespaceNodes = new List<MemberDeclarationSyntax>();
			var customTask = Configuration.Async.CustomTaskType;
			var tasksUsed = false;
			var taskConflict = false;
			var asyncLockUsed = false;
			var rewrittenNodes = new List<RewrittenNode>();

			// TODO: handle global namespace
			foreach (var namespaceInfo in DocumentInfo.Values.OrderBy(o => o.Node.SpanStart))
			{
				var namespaceNode = namespaceInfo.Node;
				taskConflict |= !DocumentInfo.ProjectInfo.IsNameUniqueInsideNamespace(namespaceInfo.Symbol, "Task");

				var typeNodes = new List<MemberDeclarationSyntax>();
				foreach (var typeInfo in namespaceInfo.Values.Where(o => o.TypeTransformation != TypeTransformation.None).OrderBy(o => o.Node.SpanStart))
				{
					if (typeInfo.CanIgnore())
					{
						continue;
					}

					var rewriteResult = RewriteType(typeInfo);
					if (rewriteResult.Node == null)
					{
						continue;
					}
					tasksUsed |= rewriteResult.TaskUsed;
					asyncLockUsed |= rewriteResult.AsyncLockUsed;
					typeNodes.Add(rewriteResult.Node);

					if (rewriteResult.ReplaceOriginalNode)
					{
						var rewritenNode = new RewrittenNode
						{
							Annotation = Guid.NewGuid().ToString(),
							Rewritten = rewriteResult.OriginalNode
						};
						var typeSpanStart = typeInfo.Node.SpanStart;
						var typeSpanLength = typeInfo.Node.Span.Length;
						var typeNode = rootNode.DescendantNodesAndSelf().OfType<TypeDeclarationSyntax>()
							.First(o => o.SpanStart == typeSpanStart && o.Span.Length == typeSpanLength);
						rootNode = rootNode.ReplaceNode(typeNode, typeNode.WithAdditionalAnnotations(new SyntaxAnnotation(rewritenNode.Annotation)));
						rewrittenNodes.Add(rewritenNode);
					}

					if (typeInfo.TypeTransformation == TypeTransformation.NewType && typeInfo.HasMissingMembers)
					{
						rewriteResult = RewriteType(typeInfo, true);
						if (rewriteResult.Node == null)
						{
							continue;
						}
						typeNodes.Add(rewriteResult.Node);
					}
				}
				if (typeNodes.Any())
				{
					namespaceNodes.Add(namespaceNode
						.WithMembers(List(typeNodes)));
				}
			}
			if (!namespaceNodes.Any())
			{
				return null;
			}

			rootNode = rootNode.WithTriviaFrom(DocumentInfo.RootNode);

			var origRootNode = rootNode;
			foreach (var rewrittenNode in rewrittenNodes)
			{
				origRootNode = rootNode.ReplaceNode(rootNode.GetAnnotatedNodes(rewrittenNode.Annotation).First(), rewrittenNode.Rewritten);
			}
			if (rootNode != origRootNode)
			{
				result.OriginalRootNode = origRootNode;
			}


			var lockNamespace = Configuration.Async.Lock.Namespace;
			if (asyncLockUsed && rootNode.Usings.All(o => o.Name.ToString() != lockNamespace))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(lockNamespace)));
			}

			if (!taskConflict && rootNode.Usings.All(o => o.Name.ToString() != "System.Threading.Tasks"))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName("System.Threading.Tasks")));
			}
			if (tasksUsed && rootNode.Usings.All(o => o.Name.ToString() != "System"))
			{
				rootNode = rootNode.AddUsings( // using Exception = System.Exception
					UsingDirective(
						QualifiedName(
							IdentifierName("System"),
							IdentifierName("Exception")))
						.WithAlias(
							NameEquals(
								IdentifierName("Exception"))));
			}
			if (tasksUsed && !string.IsNullOrEmpty(customTask.Namespace) && rootNode.Usings.All(o => o.Name.ToString() != customTask.Namespace))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(customTask.Namespace)));
			}

			if (!string.IsNullOrEmpty(Configuration.Directive))
			{
				rootNode = rootNode
					.WithLeadingTrivia(
						rootNode.GetLeadingTrivia().Insert(
							0,
							Trivia(
								IfDirectiveTrivia(
									IdentifierName(Configuration.Directive),
									true,
									false,
									false))))
					.WithEndOfFileToken(
						rootNode.EndOfFileToken.WithLeadingTrivia(
							rootNode.EndOfFileToken.LeadingTrivia.Add(
								Trivia(
									EndIfDirectiveTrivia(
										true)))));
			}

			rootNode = rootNode
					.WithMembers(List(namespaceNodes));

			//TODO: configurable custom rewriters
			var rewriter = new TransactionScopeRewriter();
			rootNode = (CompilationUnitSyntax)rewriter.VisitCompilationUnit(rootNode);

			result.NewRootNode = rootNode.NormalizeWhitespace("	");
			return result;
		}
	}
}
