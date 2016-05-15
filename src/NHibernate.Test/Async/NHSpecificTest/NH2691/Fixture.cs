#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2691
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task WhenUseCountWithOrderThenCutTheOrderAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var baseQuery =
						from cat in session.Query<Cat>()orderby cat.BirthDate
						select cat;
					Assert.That(() => baseQuery.Count(), Throws.Nothing);
					await (session.Transaction.CommitAsync());
				}
		}

		[Test]
		public async Task WhenUseLongCountWithOrderThenCutTheOrderAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var baseQuery =
						from cat in session.Query<Cat>()orderby cat.BirthDate
						select cat;
					Assert.That(() => baseQuery.LongCount(), Throws.Nothing);
					await (session.Transaction.CommitAsync());
				}
		}
	}
}
#endif
