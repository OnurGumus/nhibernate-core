using System;
using System.Collections;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomCollectionType : CollectionType
	{
		public override async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			ICollectionPersister cp = session.Factory.GetCollectionPersister(Role);
			return userType.ReplaceElements(original, target, cp, owner, copyCache, session);
		}
	}
}