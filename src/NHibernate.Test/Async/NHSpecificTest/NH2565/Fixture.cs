#if NET_4_5
using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2565
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async System.Threading.Tasks.Task WhenUseLoadThenCanUsePersistToModifyAsync()
		{
			using (var scenario = new TaskSavedScenario(Sfi))
			{
				using (var s = OpenSession())
					using (var tx = s.BeginTransaction())
					{
						var task = s.Load<Task>(scenario.TaskId);
						task.Description = "Could be something nice";
						await (s.PersistAsync(task));
						Assert.That(async () => await (s.PersistAsync(task)), Throws.Nothing);
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async System.Threading.Tasks.Task WhenUseGetThenCanUsePersistToModifyAsync()
		{
			using (var scenario = new TaskSavedScenario(Sfi))
			{
				using (var s = OpenSession())
					using (var tx = s.BeginTransaction())
					{
						var task = await (s.GetAsync<Task>(scenario.TaskId));
						task.Description = "Could be something nice";
						Assert.That(async () => await (s.PersistAsync(task)), Throws.Nothing);
						await (tx.CommitAsync());
					}
			}
		}
	}
}
#endif
