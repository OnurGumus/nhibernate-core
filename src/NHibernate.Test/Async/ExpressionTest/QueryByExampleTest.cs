#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryByExampleTest : TestCase
	{
		[Test]
		public async Task TestSimpleQBEAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("hibernate", null, "ope%");
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike();
					crit.Add(ex);
					IList result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(1, result.Count);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestEnableLikeWithMatchmodeStartAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("hib", null, "open source1");
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike(MatchMode.Start);
					crit.Add(ex);
					IList result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(1, result.Count);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestEnableLikeWithMatchmodeEndAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("nate", null, "ORM tool1");
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike(MatchMode.End);
					crit.Add(ex);
					IList result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(1, result.Count);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestEnableLikeWithMatchmodeAnywhereAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("bern", null, null);
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike(MatchMode.Anywhere);
					crit.Add(ex);
					IList result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestJunctionNotExpressionQBEAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("hibernate", null, "ope%");
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike();
					crit.Add(Expression.Or(Expression.Not(ex), ex));
					IList result = crit.List();
					Assert.IsNotNull(result);
					//if ( !(dialect is HSQLDialect - h2.1 test
					Assert.AreEqual(2, result.Count, "expected 2 objects");
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestExcludingQBEAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Componentizable master = GetMaster("hibernate", null, "ope%");
					ICriteria crit = s.CreateCriteria(typeof (Componentizable));
					Example ex = Example.Create(master).EnableLike().ExcludeProperty("Component.SubComponent");
					crit.Add(ex);
					IList result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
					master = GetMaster("hibernate", "ORM tool", "fake stuff");
					crit = s.CreateCriteria(typeof (Componentizable));
					ex = Example.Create(master).EnableLike().ExcludeProperty("Component.SubComponent.SubName1");
					crit.Add(ex);
					result = crit.List();
					Assert.IsNotNull(result);
					Assert.AreEqual(1, result.Count);
					await (t.CommitAsync());
				}
		}

		private async Task InitDataAsync()
		{
			using (ISession s = OpenSession())
			{
				Componentizable master = GetMaster("hibernate", "ORM tool", "ORM tool1");
				await (s.SaveAsync(master));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Componentizable master = GetMaster("hibernate", "open source", "open source1");
				await (s.SaveAsync(master));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Componentizable master = GetMaster("hibernate", null, null);
				await (s.SaveAsync(master));
				await (s.FlushAsync());
			}
		}

		private async Task DeleteDataAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Componentizable"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
