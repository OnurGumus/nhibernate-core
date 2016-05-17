#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MassTestingNotAndDeMorganFixture : AbstractMassTestingFixture
	{
		[Test]
		public async Task Test_NotUnequalIsTheSameAsEqualAsync()
		{
			int r1 = await (RunTestAsync(x => !(x.BO1.I1 != 1), Setters<TBO1_I>(MyBO.SetBO1_I1)));
			int r2 = await (RunTestAsync(x => x.BO1.I1 == 1, Setters<TBO1_I>(MyBO.SetBO1_I1)));
			Assert.AreEqual(r1, r2);
			Assert.Greater(r1, 0);
			r1 = await (RunTestAsync(x => !(x.BO1.I1 != 1), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			r2 = await (RunTestAsync(x => x.BO1.I1 == 1, Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			Assert.AreEqual(r1, r2);
			Assert.Greater(r1, 0);
		}

		[Test]
		public async Task Test_NotEqualIsTheSameAsNotequalAsync()
		{
			// Already the following yields different results for I1 == null even though
			// it does NOT throw an exception in Linq2Objects:
			//      ... RunTest(x => x.BO1.I1 != 1, ...);
			// * In C# logic, we get null != 1 <=> true
			// * In SQL logic, we get null != 1 <=> logical-null => false
			// To exclude this case, we can either make it false in C# ...
			int r1 = await (RunTestAsync(x => x.BO1.I1 != null && x.BO1.I1 != 1, Setters<TBO1_I>(MyBO.SetBO1_I1)));
			// ... or force it to true in SQL
			int r2 = await (RunTestAsync(x => x.BO1.I1 == null || x.BO1.I1 != 1, Setters<TBO1_I>(MyBO.SetBO1_I1)));
			// Also the following condition yields different results for I1 == null even 
			// though it does NOT throw an exception in Linq2Objects:
			//      ... RunTest(x => !(x.BO1.I1 == 1), ...);
			// * In C# logic, we get !(null == 1) <=> !(false) <=> true
			// * In SQL logic, we get !(null == 1) <=> !(logical-null) <=> logical-null => false
			// Again, to exclude this case, we can either make the inner part true in C# ...
			int r3 = await (RunTestAsync(x => !(x.BO1.I1 == null || x.BO1.I1 == 1), Setters<TBO1_I>(MyBO.SetBO1_I1)));
			// ... or force it to false in SQL:
			int r4 = await (RunTestAsync(x => !(x.BO1.I1 != null && x.BO1.I1 == 1), Setters<TBO1_I>(MyBO.SetBO1_I1)));
			Assert.Greater(r1, 0);
			Assert.Greater(r2, 0);
			// We also expect the !(==) versions to return the same result as the != versions.
			Assert.AreEqual(r1, r3);
			Assert.AreEqual(r2, r4);
		}

		[Test]
		public async Task Test_DeMorganNotAndAsync()
		{
			await (RunTestAsync(x => !(x.BO1.I1 != 1 && x.BO2.J1 != 1), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
		}

		[Test]
		public async Task Test_DeMorganNotOrAsync()
		{
			int r1 = await (RunTestAsync(x => !(x.BO1.I1 != 1 || x.BO2.J1 != 1), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			int r2 = await (RunTestAsync(x => !(x.BO1.I1 != 1) && !(x.BO2.J1 != 1), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			int r3 = await (RunTestAsync(x => x.BO1.I1 == 1 && x.BO2.J1 == 1, Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			Assert.AreEqual(r1, r2);
			Assert.AreEqual(r2, r3);
			Assert.Greater(r1, 0);
		}

		[Test]
		public async Task Test_NotNotCanBeEliminatedAsync()
		{
			// The following condition does *not* return the same values if I1 and/or J1 are
			// null in Linq2Objects and in Nhib.Linq:
			//     x => x.BO1.I1 != 1 && x.BO2.J1 != 1,
			// First, assume I1 == null and J1 == 0:
			// * In C# (Linq2Objects), we get null != 1 && 0 != 1 <=> true && true <=> true
			// * In SQL (NHib.Linq), we get null != 1 && <=> logical-null && true <=> logical-null => false
			// For I1 == 0 and J1 == null we get the same problem, as the condition is symmetric.
			// To repair this, we force "SQL" to true for nulls:
			int r1 = await (RunTestAsync(x => (x.BO1.I1 == null || x.BO1.I1 != 1) && (x.BO2.J1 == null || x.BO2.J1 != 1), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			int r2 = await (RunTestAsync(x => !!((x.BO1.I1 == null || x.BO1.I1 != 1) && (x.BO2.J1 == null || x.BO2.J1 != 1)), Setters<TBO1_I, TBO2_J>(MyBO.SetBO1_I1, MyBO.SetBO2_J1)));
			Assert.Greater(r1, 0);
			Assert.AreEqual(r1, r2);
		}
	}
}
#endif
