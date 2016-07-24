#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1178
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1178";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Foo f1 = new Foo();
					f1.Word = "mono";
					f1.Number = 0;
					await (s.SaveAsync(f1));
					Foo f2 = new Foo();
					f2.Word = "mono";
					f2.Number = 1000;
					await (s.SaveAsync(f2));
					Foo f3 = new Foo();
					f3.Word = "mono";
					f3.Number = 0;
					await (s.SaveAsync(f3));
					Foo f4 = new Foo();
					f4.Word = null;
					f4.Number = 1000;
					await (s.SaveAsync(f4));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Foo"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task ExcludeNullsAndZeroesAsync()
		{
			using (ISession s = OpenSession())
			{
				Example example = Example.Create(new Foo(1000, "mono")).ExcludeZeroes().ExcludeNulls();
				IList results = await (s.CreateCriteria(typeof (Foo)).Add(example).ListAsync());
				Assert.AreEqual(1, results.Count);
			}

			using (ISession s = OpenSession())
			{
				Example example = Example.Create(new Foo(1000, "mono")).ExcludeNulls().ExcludeZeroes();
				IList results = await (s.CreateCriteria(typeof (Foo)).Add(example).ListAsync());
				Assert.AreEqual(1, results.Count);
			}
		}
	}
}
#endif
