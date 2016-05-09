using System.Collections.Generic;
using System;

namespace NHibernate
{
	[Serializable]
	public partial class EntityModeEqualityComparer : IEqualityComparer<EntityMode>
	{
		public bool Equals(EntityMode x, EntityMode y)
		{
			return x == y;
		}

		public int GetHashCode(EntityMode obj)
		{
			return (int) obj;
		}
	}
}