using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class TypedValue
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ParameterListComparer : IEqualityComparer<TypedValue>
		{
			private async Task<bool> IsEqualsAsync(IType type, ICollection x, ICollection y)
			{
				if (x == y)
					return true;
				if (x == null || y == null)
					return false;
				if (x.Count != y.Count)
					return false;
				IEnumerator xe = x.GetEnumerator();
				IEnumerator ye = y.GetEnumerator();
				while (xe.MoveNext())
				{
					ye.MoveNext();
					if (!await (type.IsEqualAsync(xe.Current, ye.Current, entityMode)))
						return false;
				}

				return true;
			}
		}
	}
}