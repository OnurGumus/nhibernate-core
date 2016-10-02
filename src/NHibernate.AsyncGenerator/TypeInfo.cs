using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace NHibernate.AsyncGenerator
{
	public class TypeInfo
	{
		public TypeInfo(NamespaceInfo namespaceInfo, TypeInfo typeInfo, INamedTypeSymbol symbol, TypeDeclarationSyntax node)
		{
			NamespaceInfo = namespaceInfo;
			ParentTypeInfo = typeInfo;
			Symbol = symbol;
			Node = node;
		}

		public TypeInfo ParentTypeInfo { get; }

		public NamespaceInfo NamespaceInfo { get; }

		public INamedTypeSymbol Symbol { get; }

		public TypeDeclarationSyntax Node { get; }

		public TypeTransformation TypeTransformation { get; internal set; }

		public bool HasMissingMembers
		{
			get
			{
				return GetDescendantTypeInfosAndSelf().Any(o => o.MethodInfos.Any(m => m.Value.Missing));
			}
		}

		public bool CanIgnore()
		{
			return !Dependencies.Any() && 
				MethodInfos.All(o => o.Value.Ignore) &&
				TypeInfos.All(o => o.Value.CanIgnore());
		}

		/// <summary>
		/// Type references that need to be renamed
		/// </summary>
		public HashSet<ReferenceLocation> TypeReferences { get; } = new HashSet<ReferenceLocation>();

		public HashSet<ReferenceLocation> Dependencies { get; } = new HashSet<ReferenceLocation>();

		public Dictionary<MethodDeclarationSyntax, MethodInfo> MethodInfos { get; } = new Dictionary<MethodDeclarationSyntax, MethodInfo>();

		public Dictionary<TypeDeclarationSyntax, TypeInfo> TypeInfos { get; } = new Dictionary<TypeDeclarationSyntax, TypeInfo>();

		public MethodInfo GetMethodInfo(IMethodSymbol symbol, MethodDeclarationSyntax memberNode, bool create = false, bool lazyCreate = false)
		{
			if (MethodInfos.ContainsKey(memberNode))
			{
				var memberInfo = MethodInfos[memberNode];
				if (create && !lazyCreate && memberInfo.LazyCreate)
				{
					memberInfo.LazyCreate = false;
				}
				return memberInfo;
			}
			if (!create)
			{
				return null;
			}
			var asyncMember = new MethodInfo(this, symbol, memberNode)
			{
				LazyCreate = lazyCreate
			};
			MethodInfos.Add(memberNode, asyncMember);
			if ((MethodInfos).Keys.GroupBy(o => o.WithBody(null).ToString()).Any(o => o.Count() > 1)) //TODO DELETE
			{
				throw new Exception("asdad");
			}
			return asyncMember;
		}

		public MethodInfo GetMethodInfo(MethodDeclarationSyntax memberNode, bool create = false, bool lazyCreate = false)
		{
			var methodSymbol = NamespaceInfo.DocumentInfo.SemanticModel.GetDeclaredSymbol(memberNode);

			return GetMethodInfo(methodSymbol, memberNode, create, lazyCreate);
		}

		public MethodInfo GetMethodInfo(IMethodSymbol symbol, bool create = false)
		{
			var location = symbol.Locations.Single(o => o.SourceTree.FilePath == Node.SyntaxTree.FilePath);
			var memberNode = Node.DescendantNodes()
									 .OfType<MethodDeclarationSyntax>()
									 .First(o => o.ChildTokens().SingleOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);
			return GetMethodInfo(symbol, memberNode, create);
		}

		public IEnumerable<TypeInfo> GetDescendantTypeInfosAndSelf()
		{
			foreach (var typeInfo in TypeInfos.Values)
			{
				foreach (var subTypeInfo in typeInfo.GetDescendantTypeInfosAndSelf())
				{
					yield return subTypeInfo;
				}
			}
			yield return this;
		}
	}
}
