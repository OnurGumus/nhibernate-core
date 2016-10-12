#if NET_4_5
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2280
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InvalidSqlTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CompositeKeyTestAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.Query<Organisation>().Where(o => o.Codes.Any(c => c.Key.Code == "1476")).ToListAsync());
			}
		}
	}
}
#endif
