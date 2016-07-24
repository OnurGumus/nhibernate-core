using System;
using System.Collections.Generic;
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
	public class ReturnTaskMethodRewriter : CSharpSyntaxRewriter
	{
		private TypeSyntax _returnType;
		private bool _voidMethod;
		private readonly string _completedTaskType;
		private readonly string _fromExceptionTaskType;

		public ReturnTaskMethodRewriter(AsyncCustomTaskTypeConfiguration configuration)
		{
			var configuration1 = configuration;
			_completedTaskType = configuration1.HasCompletedTask ? configuration1.TypeName : "Task";
			_fromExceptionTaskType = configuration1.HasFromException ? configuration1.TypeName : "Task";
		}

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			var wrapInTryCatch = node.Body.DescendantNodes()
				.OfType<InvocationExpressionSyntax>()
				.Any(o => !o.IsAsyncMethodInvoked());
			_returnType = node.ReturnType;
			if (node.ReturnType.DescendantTokens().Any(o => o.IsKind(SyntaxKind.VoidKeyword)))
			{
				_voidMethod = true;
			}
			node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
			if (_voidMethod && !node.EndsWithReturnStatement())
			{
				_voidMethod = true;
				node = node.WithBody(
					node.Body.AddStatements(
						ReturnStatement(
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								IdentifierName(_completedTaskType),
								IdentifierName("CompletedTask"))
							)));
			}
			return wrapInTryCatch ? node.WithBody(WrapInsideTryCatch(node.Body)) : node;
		}

		public override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
		{
			// TODO: add a declaration only if there is a throws statement
			if (node.Declaration == null)
			{
				node = node.WithDeclaration(
					CatchDeclaration(IdentifierName("Exception"))
						.WithIdentifier(
							Identifier("x")));
			}
			else if (node.Declaration.Identifier.ValueText == null)
			{
				node = node.ReplaceNode(node.Declaration, node.Declaration.WithIdentifier(Identifier("x")));
			}

			return base.VisitCatchClause(node);
		}

		public override SyntaxNode Visit(SyntaxNode node)
		{
			var expression = node as ExpressionSyntax;
			if (expression != null && expression.IsReturned())
			{
				var invocationNode = node as InvocationExpressionSyntax;
				if (invocationNode != null && invocationNode.IsAsyncMethodInvoked())
				{
					return node;
				}
				return WrapInTaskFromResult(expression);
			}
			return base.Visit(node);
		}

		public override SyntaxNode VisitThrowStatement(ThrowStatementSyntax node)
		{
			if (node.Expression == null)
			{
				var catchNode = node.Ancestors().OfType<CatchClauseSyntax>().First();
				return ReturnStatement(WrapInTaskFromException(IdentifierName(catchNode.Declaration.Identifier.ValueText)));
			}

			return ReturnStatement(WrapInTaskFromException(node.Expression));
		}

		private BlockSyntax WrapInsideTryCatch(BlockSyntax node)
		{
			return Block(
				SingletonList<StatementSyntax>(
					TryStatement(
						SingletonList(
							CatchClause()
								.WithDeclaration(
									CatchDeclaration(
										IdentifierName("Exception"))
										.WithIdentifier(
											Identifier("ex")))
								.WithBlock(
									Block(
										SingletonList<StatementSyntax>(
											ReturnStatement(
												InvocationExpression(
													MemberAccessExpression(
														SyntaxKind.SimpleMemberAccessExpression,
														IdentifierName(_fromExceptionTaskType),
														GenericName(
															Identifier("FromException"))
															.WithTypeArgumentList(
																TypeArgumentList(
																	SingletonSeparatedList(
																		_voidMethod
																			? PredefinedType(Token(SyntaxKind.ObjectKeyword))
																			: _returnType)))))
													.WithArgumentList(
														ArgumentList(
															SingletonSeparatedList(
																Argument(
																	IdentifierName("ex")))))))))))
						.WithBlock(node)));
		}

		private InvocationExpressionSyntax WrapInTaskFromResult(ExpressionSyntax node)
		{
			return InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName("Task"),
					GenericName(
						Identifier("FromResult"))
						.WithTypeArgumentList(
							TypeArgumentList(
								SingletonSeparatedList(
									_voidMethod
										? PredefinedType(Token(SyntaxKind.ObjectKeyword))
										: _returnType)))))
				.WithArgumentList(
					ArgumentList(
						SingletonSeparatedList(
							Argument(node))));
		}

		private InvocationExpressionSyntax WrapInTaskFromException(ExpressionSyntax node)
		{
			return InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName(_fromExceptionTaskType),
					GenericName(
						Identifier("FromException"))
						.WithTypeArgumentList(
							TypeArgumentList(
								SingletonSeparatedList(
									_voidMethod
										? PredefinedType(Token(SyntaxKind.ObjectKeyword))
										: _returnType)))))
				.WithArgumentList(
					ArgumentList(
						SingletonSeparatedList(
							Argument(node))));
		}

	}
}
