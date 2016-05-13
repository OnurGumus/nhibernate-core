using System;
using NHibernate.Impl;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityUniqueKey
	{
		public async Task<int> GenerateHashCodeAsync(ISessionFactoryImplementor factory)
		{
			int result = 17;
			unchecked
			{
				result = 37 * result + entityName.GetHashCode();
				result = 37 * result + uniqueKeyName.GetHashCode();
				result = 37 * result + await (keyType.GetHashCodeAsync(key, entityMode, factory));
			}

			return result;
		}

		public async Task<bool> EqualsAsync(EntityUniqueKey that)
		{
			return that == null ? false : that.EntityName.Equals(entityName) && that.UniqueKeyName.Equals(uniqueKeyName) && await (keyType.IsEqualAsync(that.key, key, entityMode));
		}
	}
}