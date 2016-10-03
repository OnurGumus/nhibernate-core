#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NHibernate.Test.NHSpecificTest.NH0000;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2218
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					for (int i = 0; i < 4; i++)
					{
						await (session.SaveAsync("Entity1", new Entity{Name = "Mapping1 -" + i}));
					}

					for (int i = 0; i < 3; i++)
					{
						await (session.SaveAsync("Entity2", new Entity{Name = "Mapping2 -" + i}));
					}

					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
