#if NET_4_5
using System;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class CollectionKey
	{
		private async Task<int> GenerateHashCodeAsync()
		{
			int result = 17;
			unchecked
			{
				result = 37 * result + role.GetHashCode();
				result = 37 * result + await (keyType.GetHashCodeAsync(key, entityMode, factory));
			}

			return result;
		}
	}
}
#endif
