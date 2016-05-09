using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OptimizerFactory
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		/// <summary>
		/// Optimizer which uses a pool of values, storing the next low value of the range in the database.
		/// <para>
		/// Note that this optimizer works essentially the same as the HiLoOptimizer, except that here the
		/// bucket ranges are actually encoded into the database structures.
		/// </para>
		/// <para>
		/// Note that if you prefer that the database value be interpreted as the bottom end of our current
		/// range, then use the PooledLoOptimizer strategy.
		/// </para>
		/// </summary>
		public partial class PooledOptimizer : OptimizerSupport, IInitialValueAwareOptimizer
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override async Task<object> GenerateAsync(IAccessCallback callback)
			{
				if (_hiValue < 0)
				{
					_value = await (callback.GetNextValueAsync());
					if (_value < 1)
					{
						// unfortunately not really safe to normalize this
						// to 1 as an initial value like we do the others
						// because we would not be able to control this if
						// we are using a sequence...
						Log.Info("pooled optimizer source reported [" + _value + "] as the initial value; use of 1 or greater highly recommended");
					}

					if ((_initialValue == -1 && _value < IncrementSize) || _value == _initialValue)
						_hiValue = await (callback.GetNextValueAsync());
					else
					{
						_hiValue = _value;
						_value = _hiValue - IncrementSize;
					}
				}
				else if (_value >= _hiValue)
				{
					_hiValue = await (callback.GetNextValueAsync());
					_value = _hiValue - IncrementSize;
				}

				return Make(_value++);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		/// <summary>
		/// Common support for optimizer implementations.
		/// </summary>
		public abstract partial class OptimizerSupport : IOptimizer
		{
			public abstract Task<object> GenerateAsync(IAccessCallback param);
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class HiLoOptimizer : OptimizerSupport
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override async Task<object> GenerateAsync(IAccessCallback callback)
			{
				if (_lastSourceValue < 0)
				{
					_lastSourceValue = await (callback.GetNextValueAsync());
					while (_lastSourceValue <= 0)
					{
						_lastSourceValue = await (callback.GetNextValueAsync());
					}

					// upperLimit defines the upper end of the bucket values
					_upperLimit = (_lastSourceValue * IncrementSize) + 1;
					// initialize value to the low end of the bucket
					_value = _upperLimit - IncrementSize;
				}
				else if (_upperLimit <= _value)
				{
					_lastSourceValue = await (callback.GetNextValueAsync());
					_upperLimit = (_lastSourceValue * IncrementSize) + 1;
				}

				return Make(_value++);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NoopOptimizer : OptimizerSupport
		{
			public override async Task<object> GenerateAsync(IAccessCallback callback)
			{
				// We must use a local variable here to avoid concurrency issues.
				// With the local value we can avoid synchronizing the whole method.
				long val = -1;
				while (val <= 0)
					val = await (callback.GetNextValueAsync());
				// This value is only stored for easy access in test. Should be no
				// threading concerns there.
				_lastSourceValue = val;
				return Make(val);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PooledLoOptimizer : OptimizerSupport
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			public override async Task<object> GenerateAsync(IAccessCallback callback)
			{
				if (_lastSourceValue < 0 || _value >= (_lastSourceValue + IncrementSize))
				{
					_lastSourceValue = await (callback.GetNextValueAsync());
					_value = _lastSourceValue;
					// handle cases where initial-value is less than one (hsqldb for instance).
					while (_value < 1)
						_value++;
				}

				return Make(_value++);
			}
		}
	}
}