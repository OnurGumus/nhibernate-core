#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1831
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Oracle8iDialect);
		}

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
