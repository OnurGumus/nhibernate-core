#if NET_4_5
using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class XmlAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public abstract partial class XmlGetter : IGetter
		{
			public virtual Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
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
		}
	}
}
#endif
