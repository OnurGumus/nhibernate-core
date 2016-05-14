#if NET_4_5
using System;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class EntityKey
	{
		private async Task<int> GenerateHashCodeAsync()
		{
			int result = 17;
			unchecked
			{
				result = 37 * result + rootEntityName.GetHashCode();
				result = 37 * result + await (identifierType.GetHashCodeAsync(identifier, entityMode, factory));
			}

			return result;
		}
	}
}
#endif
