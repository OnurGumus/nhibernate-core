using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	internal class FutureValueAsync<T> : IFutureValueAsync<T>, IDelayedValue
	{
		public delegate Task<IEnumerable<T>> GetResult();

		private readonly GetResult getResult;

		public FutureValueAsync(GetResult result)
		{
			getResult = result;
		}

		public async Task<T> GetValue()
		{
			var result = await getResult().ConfigureAwait(false);
			var enumerator = result.GetEnumerator();

			if (!enumerator.MoveNext())
			{
				var defVal = default(T);
				if (ExecuteOnEval != null)
					defVal = (T)ExecuteOnEval.DynamicInvoke(defVal);
				return defVal;
			}

			var val = enumerator.Current;

			if (ExecuteOnEval != null)
				val = (T)ExecuteOnEval.DynamicInvoke(val);

			return val;

		}

		public Delegate ExecuteOnEval
		{
			get; set;
		}
	}
}
