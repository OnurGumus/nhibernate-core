#if NET_4_5
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IMilestoneCollection<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
	{
		Task<TValue> FindValueForAsync(TKey key);
	}
}
#endif
