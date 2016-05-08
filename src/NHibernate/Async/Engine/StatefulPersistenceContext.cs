using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatefulPersistenceContext : IPersistenceContext, ISerializable, IDeserializationCallback
	{
		public async Task<object[]> GetDatabaseSnapshotAsync(object id, IEntityPersister persister)
		{
			EntityKey key = session.GenerateEntityKey(id, persister);
			object cached;
			if (entitySnapshotsByKey.TryGetValue(key, out cached))
			{
				return cached == NoRow ? null : (object[])cached;
			}
			else
			{
				object[] snapshot = await (persister.GetDatabaseSnapshotAsync(id, session));
				entitySnapshotsByKey[key] = snapshot ?? NoRow;
				return snapshot;
			}
		}

		public async Task<object[]> GetNaturalIdSnapshotAsync(object id, IEntityPersister persister)
		{
			if (!persister.HasNaturalIdentifier)
			{
				return null;
			}

			// if the natural-id is marked as non-mutable, it is not retrieved during a
			// normal database-snapshot operation...
			int[] props = persister.NaturalIdentifierProperties;
			bool[] updateable = persister.PropertyUpdateability;
			bool allNatualIdPropsAreUpdateable = true;
			for (int i = 0; i < props.Length; i++)
			{
				if (!updateable[props[i]])
				{
					allNatualIdPropsAreUpdateable = false;
					break;
				}
			}

			if (allNatualIdPropsAreUpdateable)
			{
				// do this when all the properties are updateable since there is
				// a certain likelihood that the information will already be
				// snapshot-cached.
				object[] entitySnapshot = GetDatabaseSnapshot(id, persister);
				if (entitySnapshot == NoRow)
				{
					return null;
				}

				object[] naturalIdSnapshot = new object[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					naturalIdSnapshot[i] = entitySnapshot[props[i]];
				}

				return naturalIdSnapshot;
			}
			else
			{
				return await (persister.GetNaturalIdentifierSnapshotAsync(id, session));
			}
		}
	}
}