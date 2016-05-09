using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using NHibernate.Engine;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FieldAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public sealed partial class FieldGetter : IGetter, IOptimizableGetter
		{
			public async Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				return Get(owner);
			}
		}
	}
}