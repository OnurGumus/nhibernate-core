#if NET_4_5
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MilestoneCollection<TKey, TValue> : SortedDictionary<TKey, TValue>, IMilestoneCollection<TKey, TValue> where TKey : IComparable<TKey>
	{
		public Task<TValue> FindValueForAsync(TKey key)
		{
			try
			{
				return Task.FromResult<TValue>(FindValueFor(key));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<TValue>(ex);
			}
		}
	}
}
#endif
