#if NET_4_5
using System;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2287
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task DotInStringLiteralsConstantAsync()
		{
			using (ISession session = OpenSession())
			{
				var query = string.Format("from Foo f {0}where f.", Environment.NewLine);
				Assert.That(async () => await (session.CreateQuery(query).ListAsync()), Throws.TypeOf<QuerySyntaxException>());
			}
		}
	}
}
#endif
