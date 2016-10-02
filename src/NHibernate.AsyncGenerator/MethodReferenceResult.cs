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
		private bool _ignoreCalculated;

		public MethodReferenceResult(ReferenceLocation reference, SimpleNameSyntax referenceNode, IMethodSymbol symbol, MethodInfo methodInfo)
		{
			ReferenceLocation = reference;
			ReferenceNode = referenceNode;
			Symbol = symbol;
			MethodInfo = methodInfo;
		}

		public SimpleNameSyntax ReferenceNode { get; }

		public ReferenceLocation ReferenceLocation { get; }

		public IMethodSymbol Symbol { get; }

		public MethodInfo MethodInfo { get; }

		public bool UserIgnore { get; set; }

		public bool CanBeAsync { get; set; }

		public bool Ignore { get; internal set; }

		public void CalculateIgnore(int deep = 0, HashSet<MethodInfo> processedMethodInfos = null)
		{
			if (_ignoreCalculated)
			{
				return;
			}

			if (processedMethodInfos == null)
			{
				processedMethodInfos = new HashSet<MethodInfo>();
			}

			if (!CanBeAsync || UserIgnore)
			{
				Ignore = true;
				_ignoreCalculated = true;
				return;
			}

			var ignore = false;
			if (MethodInfo != null)
			{
				if (processedMethodInfos.Contains(MethodInfo))
				{
					// circular dependency
					//if (MethodInfo._ignoreCalculating)
					//{
						
					//}
					ignore |= MethodInfo.Ignore;
				}
				else
				{
					MethodInfo.CalculateIgnore(deep, processedMethodInfos);
					ignore |= MethodInfo.Ignore;
				}
				
			}
			if (!ignore)
			{
				Ignore = false;
				_ignoreCalculated = true;
				return;
			}
			Ignore = true;
			_ignoreCalculated = true;
		}

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
