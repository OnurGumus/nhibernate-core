using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NHibernate.AsyncGenerator
{
	public class TransactionScopeRewriter : CSharpSyntaxRewriter, ITransformerPlugin
	{
		public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
		{
			if (node.Type.ToString() != "TransactionScope")
			{
				return base.VisitObjectCreationExpression(node);
			}
			if (node.ArgumentList.Arguments.Any(o => o.Expression.ToString().StartsWith("TransactionScopeAsyncFlowOption")))
			{
				//TODO: what to for TransactionScopeAsyncFlowOption.Suppress?
				return node; // argument is already there
			}
			return node.WithArgumentList(
				node.ArgumentList.WithArguments(
					node.ArgumentList.Arguments.Add(
						Argument(
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								IdentifierName("TransactionScopeAsyncFlowOption"),
								IdentifierName("Enabled"))))));
		}

		public CompilationUnitSyntax BeforeNormalization(CompilationUnitSyntax syntax)
		{
			return (CompilationUnitSyntax)VisitCompilationUnit(syntax);
		}

		public CompilationUnitSyntax AfterNormalization(CompilationUnitSyntax syntax)
		{
			return syntax;
		}
	}
}
