#if NET_4_5
using System.Linq;
using NHibernate.DomainModel;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2203
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = sessions.OpenStatelessSession())
				using (var tx = session.BeginTransaction())
				{
					foreach (var artistName in new[]{"Foo", "Bar", "Baz", "Soz", "Tiz", "Fez"})
					{
						await (session.InsertAsync(new Artist{Name = artistName}));
					}

					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenStatelessSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete Artist").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}
	}
}
#endif
