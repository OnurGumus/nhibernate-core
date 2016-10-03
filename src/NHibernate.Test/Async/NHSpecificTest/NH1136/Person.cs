#if NET_4_5
using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1136
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Person : IEquatable<Person>
	{
		public Task<decimal> FindFeePercentageForValueAsync(int value)
		{
			return _feeMatrix.FindValueForAsync(value);
		}
	}
}
#endif
