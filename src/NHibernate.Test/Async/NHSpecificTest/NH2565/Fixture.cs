#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2565
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhenUseLoadThenCanUsePersistToModifyAsync()
		{
			using (var scenario = new TaskSavedScenario(Sfi))
			{
				using (var s = OpenSession())
					using (var tx = s.BeginTransaction())
					{
						var task = await (s.LoadAsync<Task>(scenario.TaskId));
						task.Description = "Could be something nice";
						await (s.PersistAsync(task));
						Assert.That(() => s.Persist(task), Throws.Nothing);
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenUseGetThenCanUsePersistToModifyAsync()
		{
			using (var scenario = new TaskSavedScenario(Sfi))
			{
				using (var s = OpenSession())
					using (var tx = s.BeginTransaction())
					{
						var task = await (s.GetAsync<Task>(scenario.TaskId));
						task.Description = "Could be something nice";
						Assert.That(() => s.Persist(task), Throws.Nothing);
						await (tx.CommitAsync());
					}
			}
		}
	}
}
#endif
