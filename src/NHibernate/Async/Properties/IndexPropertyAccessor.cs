using System;
using System.Collections;
using System.Reflection;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IndexPropertyAccessor : IPropertyAccessor
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class IndexGetter : IGetter
		{
			public async Task<object> GetForInsertAsync(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				if (session == null)
				{
					return BackrefPropertyAccessor.Unknown;
				}
				else
				{
					return session.PersistenceContext.GetIndexInOwner(encloser.entityName, encloser.propertyName, owner, mergeMap);
				}
			}
		}
	}
}