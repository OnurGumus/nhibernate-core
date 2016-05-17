#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1948
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseDecimalScaleZeroAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person person = new Person()
					{Age = 50, ShoeSize = 10, FavouriteNumbers = new List<decimal>()
					{20, 30, 40, }, };
					await (s.SaveAsync(person));
					await (s.FlushAsync());
					tx.Rollback();
				}
		}
	}
}
#endif
