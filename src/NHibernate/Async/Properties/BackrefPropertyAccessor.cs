using System;
using System.Collections;
using System.Reflection;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BackrefPropertyAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class BackrefGetter : IGetter
		{
			public async Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				if (session == null)
				{
					return Unknown;
				}
				else
				{
					return await (session.PersistenceContext.GetOwnerIdAsync(encloser.entityName, encloser.propertyName, owner, mergeMap));
				}
			}
		}
	}
}