#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Join
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1059FixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"Join.Worker.hbm.xml", };
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Worker"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task FetchJoin_ForNH1059Async()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					// This line would fail before the fix
					await (s.CreateQuery("from Worker").ListAsync<Worker>());
				}
		}

		[Test]
		public async Task FetchJoinWithCriteria_ForNH1059Async()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					// This line would fail before the fix
					await (s.CreateCriteria(typeof (Worker)).ListAsync<Worker>());
				}
		}
	}
}
#endif
