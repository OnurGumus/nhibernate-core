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

		public Dictionary<MethodDeclarationSyntax, MethodInfo> MethodInfos { get; } = new Dictionary<MethodDeclarationSyntax, MethodInfo>();

		public Dictionary<TypeDeclarationSyntax, TypeInfo> TypeInfos { get; } = new Dictionary<TypeDeclarationSyntax, TypeInfo>();

		public MethodInfo GetMethodInfo(IMethodSymbol symbol, bool create = false)
		{
			var location = symbol.Locations.Single(o => o.SourceTree.FilePath == Node.SyntaxTree.FilePath);
			var memberNode = Node.DescendantNodes()
									 .OfType<MethodDeclarationSyntax>()
									 .First(o => o.ChildTokens().SingleOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);
			if (MethodInfos.ContainsKey(memberNode))
			{
				return MethodInfos[memberNode];
			}
			if (!create)
			{
				return null;
			}
			var asyncMember = new MethodInfo(this, symbol, memberNode);
			MethodInfos.Add(memberNode, asyncMember);
			if ((MethodInfos).Keys.GroupBy(o => o.Identifier.ValueText).Any(o => o.Count() > 1))
			{

			}
			return asyncMember;
		}

	}
}
