// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static partial class SyntaxExtensions
	{

		public static SyntaxNode AddAsync(this SyntaxNode oldNode)
		{
			var nodeToModify = GetContainingMember(oldNode);
			return nodeToModify == null ? oldNode : ConvertToAsync(nodeToModify) ?? oldNode;
		}

		public static T AddAsync<T>(this SyntaxNode oldNode, T root)
			where T : SyntaxNode
		{
			var nodeToModify = GetContainingMember(oldNode);
			if (nodeToModify == null)
			{
				return null;
			}

			var modifiedNode = ConvertToAsync(nodeToModify);
			if (modifiedNode == oldNode)
			{
				return root;
			}
			if (modifiedNode != null)
			{
				return root.ReplaceNode(nodeToModify, modifiedNode);
			}

			return null;
		}

		private static SyntaxNode GetContainingMember(SyntaxNode oldNode)
		{
			foreach (var node in oldNode.AncestorsAndSelf())
			{
				switch (node.Kind())
				{
					case SyntaxKind.ParenthesizedLambdaExpression:
					case SyntaxKind.SimpleLambdaExpression:
					case SyntaxKind.AnonymousMethodExpression:
						if ((node as AnonymousFunctionExpressionSyntax)?.AsyncKeyword.Kind() != SyntaxKind.AsyncKeyword)
						{
							return node;
						}
						break;
					case SyntaxKind.MethodDeclaration:
						if ((node as MethodDeclarationSyntax)?.Modifiers.Any(SyntaxKind.AsyncKeyword) == false)
						{
							return node;
						}
						break;
					default:
						continue;
				}
			}

			return null;
		}

		private static SyntaxNode ConvertMethodToAsync(SyntaxNode methodNode)
		{
			return AddAsyncKeyword(methodNode);
		}

		private static SyntaxNode ConvertToAsync(SyntaxNode node)
		{
			return node.TypeSwitch(
				(MethodDeclarationSyntax methodNode) => ConvertMethodToAsync(methodNode),
				(ParenthesizedLambdaExpressionSyntax parenthesizedLambda) => ConvertParenthesizedLambdaToAsync(parenthesizedLambda),
				(SimpleLambdaExpressionSyntax simpleLambda) => ConvertSimpleLambdaToAsync(simpleLambda),
				(AnonymousMethodExpressionSyntax anonymousMethod) => ConvertAnonymousMethodToAsync(anonymousMethod),
				@default => null);
		}

		private static SyntaxNode ConvertParenthesizedLambdaToAsync(ParenthesizedLambdaExpressionSyntax parenthesizedLambda)
		{
			return SyntaxFactory.ParenthesizedLambdaExpression(
								SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
								parenthesizedLambda.ParameterList,
								parenthesizedLambda.ArrowToken,
								parenthesizedLambda.Body)
								.WithTriviaFrom(parenthesizedLambda)
								.WithAdditionalAnnotations(Formatter.Annotation);
		}

		private static SyntaxNode ConvertSimpleLambdaToAsync(SimpleLambdaExpressionSyntax simpleLambda)
		{
			return SyntaxFactory.SimpleLambdaExpression(
								SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
								simpleLambda.Parameter,
								simpleLambda.ArrowToken,
								simpleLambda.Body)
								.WithTriviaFrom(simpleLambda)
								.WithAdditionalAnnotations(Formatter.Annotation);
		}

		private static SyntaxNode ConvertAnonymousMethodToAsync(AnonymousMethodExpressionSyntax anonymousMethod)
		{
			return SyntaxFactory.AnonymousMethodExpression(
								SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
								anonymousMethod.DelegateKeyword,
								anonymousMethod.ParameterList,
								anonymousMethod.Block)
								.WithTriviaFrom(anonymousMethod)
								.WithAdditionalAnnotations(Formatter.Annotation);
		}

		private static SyntaxNode AddAsyncKeyword(SyntaxNode node)
		{
			var methodNode = node as MethodDeclarationSyntax;
			if (methodNode == null)
			{
				return null;
			}

			return methodNode
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
				.WithAdditionalAnnotations(Formatter.Annotation);
		}
	}
}
