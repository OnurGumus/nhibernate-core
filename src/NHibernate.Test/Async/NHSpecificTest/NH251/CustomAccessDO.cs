#if NET_4_5
using System.Collections;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Properties;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH251
{
	/// <summary>
	/// Custom access strategy that uses IDynamicFieldContainer to get/set property values
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DictionaryAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class CustomGetter : IGetter
		{
			public Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				try
				{
					return Task.FromResult<object>(GetForInsert(owner, mergeMap, session));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		// Optional operation (return null)
		}
	}
}
#endif
