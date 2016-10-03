#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1715
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from ClassA"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TimeSpanLargerThan2hAsync()
		{
			var time = new TimeSpan(0, 2, 1, 0);
			var entity = new ClassA{Time = time};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				Assert.AreEqual(time, (await (s.GetAsync<ClassA>(entity.Id))).Time);
			}
		}

		[Test]
		public async Task VerifyDaysShouldBeZeroInSmallTimeSpanAsync()
		{
			var time = new TimeSpan(1, 0, 0);
			var entity = new ClassA{Time = time};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				Assert.AreEqual(0, (await (s.GetAsync<ClassA>(entity.Id))).Time.Days);
			}
		}
	}
}
#endif
