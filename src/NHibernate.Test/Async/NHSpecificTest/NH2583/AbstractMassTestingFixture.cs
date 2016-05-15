#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractMassTestingFixture : BugTestCase
	{
		protected async Task<int> RunTestAsync<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<MyBO, bool>> condition, SetterTuple<T1, T2, T3, T4, T5, T6, T7> setters)
		{
			if (condition == null)
			{
				throw new ArgumentNullException("condition");
			}

			if (setters == null)
			{
				throw new ArgumentNullException("setters");
			}

			IEnumerable<int> expectedIds;
			// Setup
			using (var session = OpenSession())
			{
				expectedIds = await (CreateObjectsAsync(session, setters, condition.Compile()));
			}

			try
			{
				// Test
				using (var session = OpenSession())
				{
					session.CacheMode = CacheMode.Ignore;
					session.DefaultReadOnly = true;
					using (session.BeginTransaction())
					{
						return TestAndAssert(condition, session, expectedIds);
					}
				}
			}
			finally
			{
				// Teardown
				using (var session = OpenSession())
				{
					using (var tx = session.BeginTransaction())
					{
						await (DeleteAllAsync<MyBO>(session));
						await (DeleteAllAsync<MyRef1>(session));
						await (DeleteAllAsync<MyRef2>(session));
						await (DeleteAllAsync<MyRef3>(session));
						await (tx.CommitAsync());
					}
				}
			}
		}

		private static async Task DeleteAllAsync<T>(ISession session)
		{
			await (session.CreateQuery("delete from " + typeof (T).Name).ExecuteUpdateAsync());
		}

		private static async Task<IEnumerable<int>> CreateObjectsAsync<T1, T2, T3, T4, T5, T6, T7>(ISession session, SetterTuple<T1, T2, T3, T4, T5, T6, T7> setters, Func<MyBO, bool> condition)
		{
			var expectedIds = new List<int>();
			bool thereAreSomeWithTrue = false;
			bool thereAreSomeWithFalse = false;
			var allTestCases = GetAllTestCases<T1, T2, T3, T4, T5, T6, T7>().ToList();
			var i = 0;
			foreach (var q in allTestCases)
			{
				MyBO bo = new MyBO();
				setters.Set(bo, session, q.Item1, q.Item2, q.Item3, q.Item4, q.Item5, q.Item6, q.Item7);
				try
				{
					if (condition(bo))
					{
						expectedIds.Add(bo.Id);
						thereAreSomeWithTrue = true;
					}
					else
					{
						thereAreSomeWithFalse = true;
					}

					if ((i % BatchSize) == 0)
					{
						if (session.Transaction.IsActive)
						{
							await (session.Transaction.CommitAsync());
							session.Clear();
						}

						session.BeginTransaction();
					}

					await (session.SaveAsync(bo));
					i++;
				}
				catch (NullReferenceException)
				{
				// ignore - we only check consistency with Linq2Objects in non-failing cases;
				// emulating the outer-join logic for exceptional cases in Lin2Objects is IMO very hard.
				}
			}

			if (session.Transaction.IsActive)
			{
				await (session.Transaction.CommitAsync());
				session.Clear();
			}

			Console.WriteLine("Congratulation!! you have saved " + i + " entities.");
			if (!thereAreSomeWithTrue)
			{
				throw new ArgumentException("Condition is false for all - not a good test", "condition");
			}

			if (!thereAreSomeWithFalse)
			{
				throw new ArgumentException("Condition is true for all - not a good test", "condition");
			}

			return expectedIds;
		}
	}
}
#endif
