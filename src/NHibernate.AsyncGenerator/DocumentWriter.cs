using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

		private MethodDeclarationSyntax RewiteMethod(MethodInfo methodInfo, MethodAnalyzeResult analyzeResult)
		{
			if (!analyzeResult.HasBody)
			{
				return methodInfo.Node
								 .WithoutAttribute("Async")
								 .ReturnAsTask(methodInfo.Symbol)
								 .WithIdentifier(Identifier(methodInfo.Node.Identifier.Value + "Async"));
			}

			var methodNode = methodInfo.Node.WithoutTrivia(); // references have spans without trivia
			
			var awaitsPlacements = new Dictionary<int, int>(); // key is SpanStart and value is the length of the await
			foreach (var referenceResult in analyzeResult.ReferenceResults
				.Where(o => o.CanBeAsync)
				.OrderByDescending(o => o.ReferenceLocation.Location.SourceSpan.Start))
			{
				var reference = referenceResult.ReferenceLocation;
				var startSpan = reference.Location.SourceSpan.Start - methodInfo.Node.Span.Start;
				var nameNode = methodNode.Body.DescendantNodes()
										 .OfType<SimpleNameSyntax>()
										 .FirstOrDefault(
											 o =>
											 {
												 if (!o.IsKind(SyntaxKind.GenericName))
												 {
													 return o.Span.Start == startSpan && o.Span.Length == reference.Location.SourceSpan.Length;
												 }
												 var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
												 return token.Span.Start == startSpan && token.Span.Length == reference.Location.SourceSpan.Length;
											 });
				if (nameNode == null) // previous awaits were placed before current, caluclate the difference
				{
					var newStartSpan = startSpan;
					foreach (var pair in awaitsPlacements.OrderBy(o => o.Key))
					{
						if (pair.Key > startSpan)
						{
							break;
						}
						newStartSpan += pair.Value;
					}
					nameNode = methodNode.Body.DescendantNodes()
										 .OfType<SimpleNameSyntax>()
										 .First(
											 o =>
											 {
												 if (!o.IsKind(SyntaxKind.GenericName))
												 {
													 return o.Span.Start == newStartSpan && o.Span.Length == reference.Location.SourceSpan.Length;
												 }
												 var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
												 return token.Span.Start == newStartSpan && token.Span.Length == reference.Location.SourceSpan.Length;
											 });
				}

				if (analyzeResult.CanSkipAsync)
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
					var awaitLength = awaitNode.IsKind(SyntaxKind.ParenthesizedExpression) ? 7 : 6;
					methodNode = methodNode.ReplaceNode(invocationNode, awaitNode);
					awaitNode = methodNode.GetAnnotatedNodes(lastAwaitAnnotation).Single();
					awaitsPlacements.Add(awaitNode.SpanStart, awaitLength);
				}
			}

			if (!analyzeResult.CanBeAsnyc && !analyzeResult.IsEmpty)
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
			else if (analyzeResult.Yields)
			{
				var yieldRewriter = new YieldToAsyncMethodRewriter();
				methodNode = (MethodDeclarationSyntax)yieldRewriter.VisitMethodDeclaration(methodNode);
			}

			if (!analyzeResult.CanBeAsnyc || analyzeResult.CanSkipAsync || analyzeResult.Yields)
			{
				var taskRewriter = new ReturnTaskMethodRewriter(Configuration.Async.CustomTaskType);
				methodNode = (MethodDeclarationSyntax)taskRewriter.VisitMethodDeclaration(methodNode);
			}

			// check if method must run synhronized
			if (analyzeResult.MustRunSynchronized)
			{
				methodNode = methodNode
					.WithBody(AsyncLockBlock(methodNode.Body))
					.WithoutAttribute("MethodImpl");
			}
			methodNode = methodNode
				.WithoutAttribute("Async")
				.ReturnAsTask(methodInfo.Symbol)
				.WithIdentifier(Identifier(methodNode.Identifier.Value + "Async"));

			if (analyzeResult.MustRunSynchronized || (!analyzeResult.CanSkipAsync && !analyzeResult.Yields && analyzeResult.CanBeAsnyc))
			{
				methodNode = methodNode.AddAsync(methodNode);
			}
			return methodNode
				.WithLeadingTrivia(methodInfo.Node.GetLeadingTrivia())
				.WithTrailingTrivia(methodInfo.Node.GetTrailingTrivia());
		}

		public void Write()
		{
			var rootNode = DocumentInfo.RootNode;
			var namespaceNodes = new List<MemberDeclarationSyntax>();
			var customTask = Configuration.Async.CustomTaskType;
			var tasksUsed = false;
			foreach (var namespaceInfo in DocumentInfo.NamespaceInfos.Values.OrderBy(o => o.Node.SpanStart))
			{
				var namespaceNode = namespaceInfo.Node;
				var typeNodes = new List<MemberDeclarationSyntax>();
				foreach (var typeInfo in namespaceInfo.TypeInfos.Values.OrderBy(o => o.Node.SpanStart))
				{
					var typeNode = typeInfo.Node;
					var asyncLock = false;
					var memberNodes = new List<MemberDeclarationSyntax>();
					foreach (var methodInfo in typeInfo.MethodInfos.Values.OrderBy(o => o.Node.SpanStart))
					{
						var methodAnalyzeResult = methodInfo.Analyze();
						tasksUsed |= methodAnalyzeResult.CanSkipAsync || methodAnalyzeResult.IsEmpty || methodAnalyzeResult.Yields || !methodAnalyzeResult.CanBeAsnyc;
						asyncLock |= methodAnalyzeResult.MustRunSynchronized;
						memberNodes.Add(RewiteMethod(methodInfo, methodAnalyzeResult));
					}
					// TODO: inifinite levels of nested types
					foreach (var subTypeInfo in typeInfo.TypeInfos.Values.OrderBy(o => o.Node.SpanStart))
					{
						var subTypeNode = subTypeInfo.Node;
						var subAsyncLock = false;
						var subMemberNodes = new List<MemberDeclarationSyntax>();
						foreach (var methodInfo in subTypeInfo.MethodInfos.Values.OrderBy(o => o.Node.SpanStart))
						{
							var methodAnalyzeResult = methodInfo.Analyze();
							tasksUsed |= methodAnalyzeResult.CanSkipAsync || methodAnalyzeResult.IsEmpty || methodAnalyzeResult.Yields || !methodAnalyzeResult.CanBeAsnyc;
							subAsyncLock |= methodAnalyzeResult.MustRunSynchronized;
							subMemberNodes.Add(RewiteMethod(methodInfo, methodAnalyzeResult));
						}
						if (subAsyncLock)
						{
							var lockNamespace = Configuration.Async.Lock.Namespace;
							subMemberNodes.Insert(0, GetAsyncLockField());
							if (rootNode.Usings.All(o => o.Name.ToString() != lockNamespace))
							{
								rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(lockNamespace)));
							}
						}
						subTypeNode = subTypeNode
							.AddPartial()
							.WithMembers(List(subMemberNodes));
						subTypeNode = subTypeInfo.Symbol.Locations
												.Where(o => DocumentInfo.ProjectInfo.ContainsKey(o.SourceTree.FilePath))
												.Select(o => DocumentInfo.ProjectInfo[o.SourceTree.FilePath].GetTypeInfo(subTypeInfo.Symbol).Node)
												.Any(n => n.AttributeLists.Any(o => o.Attributes.Any(a => a.Name.ToString().EndsWith("GeneratedCode"))))
							? subTypeNode.WithoutAttributes()
							: subTypeNode.AddGeneratedCodeAttribute(false);
						memberNodes.Add(subTypeNode);
					}

					// add partial to the original file
					//var partialTypeDeclaration = typeDeclaration.AddPartial();
					//if (partialTypeDeclaration != typeDeclaration)
					//{
					//	partialTypeDeclaration = partialTypeDeclaration.NormalizeWhitespace("	");
					//	rootChanged = true;
					if (asyncLock)
					{
						var lockNamespace = Configuration.Async.Lock.Namespace;
						memberNodes.Insert(0, GetAsyncLockField());
						if (rootNode.Usings.All(o => o.Name.ToString() != lockNamespace))
						{
							rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(lockNamespace)));
						}
					}
					typeNode = typeNode
						.AddPartial()
						.WithMembers(List(memberNodes));
					typeNode = typeInfo.Symbol.Locations
									   .Where(o => DocumentInfo.ProjectInfo.ContainsKey(o.SourceTree.FilePath))
									   .Select(o => DocumentInfo.ProjectInfo[o.SourceTree.FilePath].GetTypeInfo(typeInfo.Symbol).Node)
									   .Any(n => n.AttributeLists.Any(o => o.Attributes.Any(a => a.Name.ToString().EndsWith("GeneratedCode"))))
						? typeNode.WithoutAttributes()
						: typeNode.AddGeneratedCodeAttribute(false);
					typeNode = typeNode.RemoveNodes(
							typeNode.DescendantNodes(descendIntoTrivia: true).OfType<DirectiveTriviaSyntax>(), SyntaxRemoveOptions.KeepNoTrivia); // remove invalid #endregion
					typeNodes.Add(typeNode);
				}
				namespaceNodes.Add(namespaceNode
						.WithMembers(List(typeNodes)));
			}

			if (rootNode.Usings.All(o => o.Name.ToString() != "System.Threading.Tasks"))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName("System.Threading.Tasks")));
			}
			if (tasksUsed && rootNode.Usings.All(o => o.Name.ToString() != "System"))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName("System")));
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

			var content = rootNode
				.WithMembers(List(namespaceNodes))
				.NormalizeWhitespace("	")
				.ToFullString();

			if (!Directory.Exists(DestinationFolder))
			{
				Directory.CreateDirectory(DestinationFolder);
			}
			File.WriteAllText($"{DestinationFolder}\\{DocumentInfo.Name}", content);
		}
	}
}
