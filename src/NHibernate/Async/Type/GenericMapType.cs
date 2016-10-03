#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GenericMapType<TKey, TValue> : CollectionType
	{
		public override async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			ICollectionPersister cp = session.Factory.GetCollectionPersister(Role);
			IDictionary<TKey, TValue> result = (IDictionary<TKey, TValue>)target;
			result.Clear();
			IEnumerable<KeyValuePair<TKey, TValue>> iter = (IDictionary<TKey, TValue>)original;
			foreach (KeyValuePair<TKey, TValue> me in iter)
			{
				TKey key = (TKey)await (cp.IndexType.ReplaceAsync(me.Key, null, session, owner, copyCache));
				TValue value = (TValue)await (cp.ElementType.ReplaceAsync(me.Value, null, session, owner, copyCache));
				result[key] = value;
			}

			var originalPc = original as IPersistentCollection;
			var resultPc = result as IPersistentCollection;
			if (originalPc != null && resultPc != null)
			{
				if (!originalPc.IsDirty)
					resultPc.ClearDirty();
			}

			return result;
		}
	}
}
#endif
