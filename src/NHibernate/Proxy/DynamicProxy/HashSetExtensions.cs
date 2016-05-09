using System.Collections.Generic;

namespace NHibernate.Proxy.DynamicProxy
{
	public static partial class HashSetExtensions
	{
		public static HashSet<T> Merge<T>(this HashSet<T> source, IEnumerable<T> toMerge)
		{
			foreach (T item in toMerge)
			{
				source.Add(item);
			}
			return source;
		}
	}
}