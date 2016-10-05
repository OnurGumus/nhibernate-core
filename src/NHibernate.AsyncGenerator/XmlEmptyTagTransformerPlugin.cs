using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static NHibernate.AsyncGenerator.Extensions.SyntaxHelper;

namespace NHibernate.AsyncGenerator
{
	/// <summary>
	/// NormalizeWhitespace has a problem when formatting an empty xml tag that spans over multiple lines
	/// This class will save the current node before normalization and then will apply the old trivia to the normalized node
	/// </summary>
	public class XmlEmptyTagTransformerPlugin : ITransformerPlugin
	{
		private readonly DocumentTransformer _transformer;
		private readonly Dictionary<string, SyntaxNode> _nodes = new Dictionary<string, SyntaxNode>();

		public XmlEmptyTagTransformerPlugin(DocumentTransformer transformer)
		{
			_transformer = transformer;
		}

		public CompilationUnitSyntax BeforeNormalization(CompilationUnitSyntax rootNode)
		{
			var origLeadingTrivia = rootNode.GetLeadingTrivia();
			var origTrailingTrivia = rootNode.GetTrailingTrivia();
			rootNode = rootNode.WithoutTrivia();
			var list = rootNode.DescendantNodes()
							   .OfType<MemberDeclarationSyntax>()
							   .Where(
								   o => o.HasLeadingTrivia &&
										o.GetLeadingTrivia()
										 .Any(
											 t =>
												 t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) &&
												 ((DocumentationCommentTriviaSyntax)t.GetStructure()).Content.OfType<XmlEmptyElementSyntax>()
													.Any(x =>
														x.SyntaxTree.GetLineSpan(x.LessThanToken.Span).StartLinePosition.Line !=
														x.SyntaxTree.GetLineSpan(x.SlashGreaterThanToken.Span).StartLinePosition.Line)
									   ))
							   .ToList();
			foreach (var memberNode in list)
			{
				var typeSpanStart = memberNode.SpanStart;
				var typeSpanLength = memberNode.Span.Length;
				var node = rootNode.DescendantNodesAndSelf().OfType<MemberDeclarationSyntax>()
					.First(o => o.SpanStart == typeSpanStart && o.Span.Length == typeSpanLength);

				var annotation = Guid.NewGuid().ToString();
				rootNode = rootNode.ReplaceNode(node, node.WithAdditionalAnnotations(new SyntaxAnnotation(annotation)));
				_nodes.Add(annotation, node);
			}

			return rootNode
				.WithLeadingTrivia(origLeadingTrivia)
				.WithTrailingTrivia(origTrailingTrivia);
		}

		public CompilationUnitSyntax AfterNormalization(CompilationUnitSyntax rootNode)
		{
			foreach (var pair in _nodes)
			{
				var node = rootNode.GetAnnotatedNodes(pair.Key).OfType<MemberDeclarationSyntax>().First();
				var oldNode = pair.Value;
				var oldLeadingTrivia = oldNode.GetLeadingTrivia();
				var commentIdx = oldLeadingTrivia.IndexOf(SyntaxKind.SingleLineDocumentationCommentTrivia);
				var commentNode = (DocumentationCommentTriviaSyntax)oldLeadingTrivia[commentIdx].GetStructure();

				var emptyElem = commentNode.Content.OfType<XmlEmptyElementSyntax>().First();
				var fileNameAttr = emptyElem.Attributes.OfType<XmlTextAttributeSyntax>().FirstOrDefault(o => o.Name.ToString() == "file");
				if (fileNameAttr == null)
				{
					continue;
				}
				var textToken = fileNameAttr.TextTokens.First();
				var newPath = _transformer.RelativePathToOriginal + @"\" + textToken.ValueText;
				var newFileNameAttr = fileNameAttr.WithTextTokens(
						TokenList(XmlTextLiteral(
							textToken.LeadingTrivia,
							newPath,
							newPath,
							textToken.TrailingTrivia)));
				oldLeadingTrivia = oldLeadingTrivia.Replace(
					oldLeadingTrivia[commentIdx],
					Trivia(commentNode.ReplaceNode(fileNameAttr, newFileNameAttr)));
				rootNode = rootNode.ReplaceNode(node, node.WithLeadingTrivia(oldLeadingTrivia));
			}
			return rootNode;
		}
	}
}
