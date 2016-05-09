using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine.Loading
{
	/// <summary> 
	/// Maps <see cref = "IDataReader"/> to specific contextual data
	/// related to processing that <see cref = "IDataReader"/>.
	/// </summary>
	/// <remarks>
	/// Implementation note: internally an <see cref = "IdentityMap"/> is used to maintain
	/// the mappings; <see cref = "IdentityMap"/> was chosen because I'd rather not be
	/// dependent upon potentially bad <see cref = "IDataReader"/> and <see cref = "IDataReader"/>
	/// implementations.
	/// Considering the JDBC-redesign work, would further like this contextual info
	/// not mapped separately, but available based on the result set being processed.
	/// This would also allow maintaining a single mapping as we could reliably get
	/// notification of the result-set closing...
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LoadContexts
	{
		public async Task<IPersistentCollection> LocateLoadingCollectionAsync(ICollectionPersister persister, object ownerKey)
		{
			LoadingCollectionEntry lce = LocateLoadingCollectionEntry(new CollectionKey(persister, ownerKey, Session.EntityMode));
			if (lce != null)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("returning loading collection:" + await (MessageHelper.CollectionInfoStringAsync(persister, ownerKey, Session.Factory)));
				}

				return lce.Collection;
			}
			else
			{
				return null;
			}
		}
	}
}