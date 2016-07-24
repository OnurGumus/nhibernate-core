#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2488
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
#region Scenarios
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class FetchSelectScenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public FetchSelectScenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var entity = new Derived1{ShortContent = "Short", LongContent = "LongLongLongLongLong", };
						s.Save(entity);
						t.Commit();
					}
				}
			}

			public void Dispose()
			{
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						s.Delete("from Derived1");
						t.Commit();
					}
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class FetchJoinScenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public FetchJoinScenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var entity = new Derived2{ShortContent = "Short", LongContent = "LongLongLongLongLong", };
						s.Save(entity);
						t.Commit();
					}
				}
			}

			public void Dispose()
			{
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						s.Delete("from Derived2");
						t.Commit();
					}
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class JoinedSubclassScenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public JoinedSubclassScenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var entity = new Derived3{ShortContent = "Short", LongContent = "LongLongLongLongLong", };
						s.Save(entity);
						t.Commit();
					}
				}
			}

			public void Dispose()
			{
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						s.Delete("from Derived3");
						t.Commit();
					}
				}
			}
		}

#endregion
		[Test]
		public async Task ShouldNotQueryLazyProperties_FetchJoinAsync()
		{
			using (new FetchJoinScenario(Sfi))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						IList<Base2> items;
						using (var ls = new SqlLogSpy())
						{
							items = await (s.CreateQuery("from Base2").ListAsync<Base2>());
							Assert.That(ls.GetWholeLog(), Is.Not.StringContaining("LongContent"));
						}

						var item = (Derived2)items[0];
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.False);
						string lc = item.LongContent;
						Assert.That(lc, Is.Not.Null.And.Not.Empty);
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.True);
					}
				}
			}
		}

		[Test]
		public async Task ShouldNotQueryLazyProperties_FetchSelectAsync()
		{
			using (new FetchSelectScenario(Sfi))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						IList<Base1> items;
						using (var ls = new SqlLogSpy())
						{
							items = await (s.CreateQuery("from Base1").ListAsync<Base1>());
							Assert.That(ls.GetWholeLog(), Is.Not.StringContaining("LongContent"));
						}

						var item = (Derived1)items[0];
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.False);
						string lc = item.LongContent;
						Assert.That(lc, Is.Not.Null.And.Not.Empty);
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.True);
					}
				}
			}
		}

		[Test]
		public async Task ShouldNotQueryLazyProperties_JoinedsubclassAsync()
		{
			using (new JoinedSubclassScenario(Sfi))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						IList<Base3> items;
						using (var ls = new SqlLogSpy())
						{
							items = await (s.CreateQuery("from Base3").ListAsync<Base3>());
							Assert.That(ls.GetWholeLog(), Is.Not.StringContaining("LongContent"));
						}

						var item = (Derived3)items[0];
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.False);
						string lc = item.LongContent;
						Assert.That(lc, Is.Not.Null.And.Not.Empty);
						Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(item, "LongContent")), Is.True);
					}
				}
			}
		}
	}
}
#endif
