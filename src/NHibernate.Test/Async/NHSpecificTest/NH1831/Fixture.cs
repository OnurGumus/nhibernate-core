#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1831
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CorrectPrecedenceForBitwiseOperatorsAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					const string hql = @"SELECT dt FROM DocumentType dt WHERE dt.systemAction & :sysAct = :sysAct ";
					await (s.CreateQuery(hql).SetParameter("sysAct", SystemAction.Denunciation).ListAsync());
				}
		}
	}
}
#endif
