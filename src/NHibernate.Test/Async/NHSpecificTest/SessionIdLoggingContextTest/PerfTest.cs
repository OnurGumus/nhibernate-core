#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using NHibernate.Transaction;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.SessionIdLoggingContextTest
{
	[TestFixture, Explicit("This is a performance test and take a while.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PerfTestAsync : BugTestCaseAsync
	{
		const int noOfParents = 1000;
		const int noOfChildrenForEachParent = 20;
		[Test]
		public async Task BenchmarkAsync()
		{
			using (var s = OpenSession())
			{
				var ticksAtStart = DateTime.Now.Ticks;
				var res = await (s.CreateCriteria<ClassA>().SetFetchMode("Children", FetchMode.Join).SetResultTransformer(Transformers.DistinctRootEntity).Add(Restrictions.Eq("Name", "Parent")).ListAsync<ClassA>());
				Console.WriteLine(TimeSpan.FromTicks(DateTime.Now.Ticks - ticksAtStart));
				Assert.AreEqual(noOfParents, res.Count);
				Assert.AreEqual(noOfChildrenForEachParent, res[0].Children.Count);
			}
		}

		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
			{
				//get rid of the overhead supporting distr trans
				configuration.SetProperty(Cfg.Environment.TransactionStrategy, typeof (AdoNetTransactionFactory).FullName);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					for (var i = 0; i < noOfParents; i++)
					{
						var parent = createEntity("Parent");
						for (var j = 0; j < noOfChildrenForEachParent; j++)
						{
							var child = createEntity("Child");
							parent.Children.Add(child);
						}

						await (s.SaveAsync(parent));
					}

					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from ClassA").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}

		private static ClassA createEntity(string name)
		{
			var obj = new ClassA{Children = new List<ClassA>(), Name = name};
			return obj;
		}
	}
}
#endif
