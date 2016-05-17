#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3124
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
		[Test]
		public async Task LinqStatementGeneratesIncorrectCastToIntegerAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					session.Query<Person>().Where(x => x.Type == 'A').ToList();
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
