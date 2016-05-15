#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3641
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestFixture : BugTestCase
	{
		private static async Task DeleteAllAsync<T>(ISession session)
		{
			await (session.CreateQuery("delete from " + typeof (T).Name + " where ChildInterface is not null").ExecuteUpdateAsync());
			await (session.CreateQuery("delete from " + typeof (T).Name).ExecuteUpdateAsync());
		}
	}
}
#endif
