using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public class TypeInfo
	{
		public TypeInfo(NamespaceInfo namespaceInfo, INamedTypeSymbol symbol, TypeDeclarationSyntax node)
		{
			NamespaceInfo = namespaceInfo;
			Symbol = symbol;
			Node = node;
		}

		public NamespaceInfo NamespaceInfo { get; }

		public INamedTypeSymbol Symbol { get; }

		public TypeDeclarationSyntax Node { get; }

		public Dictionary<IMethodSymbol, MethodInfo> MethodInfos { get; } = new Dictionary<IMethodSymbol, MethodInfo>();

		public Dictionary<INamedTypeSymbol, TypeInfo> TypeInfos { get; } = new Dictionary<INamedTypeSymbol, TypeInfo>();

		public MethodInfo GetMethodInfo(IMethodSymbol symbol, bool create = false)
		{
			if (MethodInfos.ContainsKey(symbol))
			{
				return MethodInfos[symbol];
			}
			if (!create)
			{
				return null;
			}
			var location = symbol.Locations.Single(o => o.SourceTree.FilePath == Node.SyntaxTree.FilePath);
			var memberNode = Node.DescendantNodes()
									 .OfType<MethodDeclarationSyntax>()
									 .Single(o => o.ChildTokens().SingleOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);
			var asyncMember = new MethodInfo(this, symbol, memberNode);
			MethodInfos.Add(symbol, asyncMember);
			return asyncMember;
		}

	}
}
