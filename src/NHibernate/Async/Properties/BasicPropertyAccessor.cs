using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicPropertyAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public sealed partial class BasicGetter : IGetter, IOptimizableGetter
		{
			public async Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				return Get(owner);
			}
		}
	}
}