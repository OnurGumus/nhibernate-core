#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.DataReaderWrapperTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseDatareadersGetValueAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var crit = s.CreateCriteria(typeof (TheEntity));
					var multi = s.CreateMultiCriteria();
					multi.Add(crit);
					var res = (IList)(await (multi.ListAsync()))[0];
					Assert.That(res.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
