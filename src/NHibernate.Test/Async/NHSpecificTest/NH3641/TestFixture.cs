#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3641
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestFixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var child = new Entity{Id = 1, Flag = true};
					var parent = new Entity{Id = 2, ChildInterface = child, ChildConcrete = child};
					await (session.SaveAsync(child));
					await (session.SaveAsync(parent));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (DeleteAllAsync<Entity>(session));
					await (tx.CommitAsync());
				}
		}

		private static async Task DeleteAllAsync<T>(ISession session)
		{
			await (session.CreateQuery("delete from " + typeof (T).Name + " where ChildInterface is not null").ExecuteUpdateAsync());
			await (session.CreateQuery("delete from " + typeof (T).Name).ExecuteUpdateAsync());
		}
	}
}
#endif
