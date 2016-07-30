using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace NHibernate.AsyncGenerator
{
	public class MethodReferenceResult
	{
		public MethodReferenceResult(ReferenceLocation reference, SimpleNameSyntax referenceNode, IMethodSymbol symbol)
		{
			ReferenceLocation = reference;
			ReferenceNode = referenceNode;
			Symbol = symbol;
		}

		public SimpleNameSyntax ReferenceNode { get; }

		public ReferenceLocation ReferenceLocation { get; }

		public IMethodSymbol Symbol { get; }

		public bool Ignore { get; set; }

		public bool CanBeAsync { get; internal set; }

		public bool DeclaredWithinSameType { get; internal set; }

		public bool CanBeAwaited { get; internal set; } = true;

		public bool UsedAsReturnValue { get; internal set; }

		public bool LastStatement { get; internal set; }

		public bool InsideCondition { get; internal set; }

		public bool PassedAsArgument { get; internal set; }

		public bool MustBeAwaited { get; set; }

		public bool WrapInsideAsyncFunction { get; set; }

		public bool MakeAnonymousFunctionAsync { get; set; }

		public override bool Equals(object obj)
		{
			var refResult = obj as MethodReferenceResult;
			return refResult != null && refResult.ReferenceLocation.Equals(ReferenceLocation);
		}

		public override int GetHashCode()
		{
			return ReferenceLocation.GetHashCode();
		}
	}
}
