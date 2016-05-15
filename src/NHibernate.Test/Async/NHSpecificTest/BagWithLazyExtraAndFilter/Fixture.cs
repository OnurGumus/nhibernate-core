#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.BagWithLazyExtraAndFilter
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseFilterForLazyExtraAsync()
		{
			using (var s = OpenSession())
			{
				s.BeginTransaction();
				var machineRequest = new MachineRequest{EnvId = 1L, Id = 2L};
				await (s.SaveAsync(new Env{Id = 1L, RequestsFailed = new List<MachineRequest>{machineRequest}}));
				await (s.SaveAsync(machineRequest));
				await (s.Transaction.CommitAsync());
			}

			using (var s = OpenSession())
			{
				var env = await (s.LoadAsync<Env>(1L));
				Assert.AreEqual(1, env.RequestsFailed.Count);
			}

			using (var s = OpenSession())
			{
				s.EnableFilter("CurrentOnly");
				var env = await (s.LoadAsync<Env>(1L));
				Assert.AreEqual(0, env.RequestsFailed.Count);
			}

			using (var s = OpenSession())
			{
				s.BeginTransaction();
				await (s.DeleteAsync(await (s.LoadAsync<MachineRequest>(2L))));
				await (s.DeleteAsync(await (s.LoadAsync<Env>(1L))));
				await (s.Transaction.CommitAsync());
			}
		}
	}
}
#endif
