#if NET_4_5
using System;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentMilestoneCollection<TKey, TValue> : PersistentGenericMap<TKey, TValue>, IMilestoneCollection<TKey, TValue> where TKey : IComparable<TKey>
	{
		public async Task<TValue> FindValueForAsync(TKey key)
		{
			await (ReadAsync());
			return await (((IMilestoneCollection<TKey, TValue>)WrappedMap).FindValueForAsync(key));
		}
	}
}
#endif
