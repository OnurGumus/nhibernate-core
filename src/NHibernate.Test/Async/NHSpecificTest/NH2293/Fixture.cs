#if NET_4_5
using System.Linq;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2293
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return Enumerable.Empty<object>().ToList();
			}
		}

		[Test]
		public async Task WhenQueryHasJustAfromThenThrowQuerySyntaxExceptionAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.That(async () => await (session.CreateQuery("from").ListAsync()), Throws.TypeOf<QuerySyntaxException>());
			}
		}
	}
}
#endif
