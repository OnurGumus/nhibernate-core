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
	public class YieldToAsyncMethodRewriter : CSharpSyntaxRewriter
	{
		private GenericNameSyntax _returnType;

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			_returnType = (GenericNameSyntax)node.ReturnType; // IEnumerable<T>

			var varible = LocalDeclarationStatement(
				VariableDeclaration(
					IdentifierName("var"))
					.WithVariables(
						SingletonSeparatedList(
							VariableDeclarator(
								Identifier("yields"))
								.WithInitializer(
									EqualsValueClause(
										ObjectCreationExpression(
											GenericName(
												Identifier("List"))
												.WithTypeArgumentList(
													TypeArgumentList(
														SingletonSeparatedList<TypeSyntax>(_returnType
														.TypeArgumentList
														.DescendantNodes()
														.OfType<SimpleNameSyntax>()
														.First()))))
											.WithArgumentList(
												ArgumentList()))))));

			var returnStatement = ReturnStatement(IdentifierName("yields"));

			node = node.WithBody(
				Block(
					node.Body.Statements.Insert(0, varible)
						.Add(returnStatement)));
			return base.VisitMethodDeclaration(node);
		}

		public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
		{
			var addYield = ExpressionStatement(
				InvocationExpression(
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName("yields"),
						IdentifierName("Add")))
					.WithArgumentList(
						ArgumentList(
							SingletonSeparatedList(
								Argument(node.Expression)))));
			return addYield;
		}
	}
}
