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
	public partial class MassTestingMoreOperatorsFixture : AbstractMassTestingFixture
	{
		// Condition pattern: (A && B) && (C || D) SELECT E
		[Test]
		public async Task TestNestedPlusAsync()
		{
			await (RunTestAsync(x => (x.K1 + x.K2) + x.K2 == null || (x.K1 + x.K2) + x.K2 == null, Setters<TK, TK>(MyBO.SetK1, MyBO.SetK2)));
		}

		[Test]
		public async Task TestNestedPlusBehindNotAsync()
		{
			await (RunTestAsync(x => !((x.K1 + x.K2) + x.K2 != null), Setters<TK, TK>(MyBO.SetK1, MyBO.SetK2)));
		}

		[Test]
		public async Task TestNestedPlusBehindNotAndAsync()
		{
			await (RunTestAsync(x => !((x.K1 + x.K2) + x.K2 != null && (x.K1 + x.K2) + x.K2 != null), Setters<TK, TK>(MyBO.SetK1, MyBO.SetK2)));
		}

		[Test]
		public async Task TestNestedPlusBehindNotOrAsync()
		{
			await (RunTestAsync(x => !((x.K1 + x.K2) + x.K2 != null || (x.K1 + x.K2) + x.K2 != null), Setters<TK, TK>(MyBO.SetK1, MyBO.SetK2)));
		}

		[Test]
		public async Task TestNestedPlusBehindOrNavAsync()
		{
			await (RunTestAsync(x => (x.BO1.I1 + x.BO1.I2) + x.BO1.I2 == null || (x.BO1.I1 + x.BO1.I2) + x.BO1.I2 == null, await (await (Setters<TBO1_I, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async)))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotNavAsync()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO1.I2 != null), await (await (Setters<TBO1_I, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async)))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotAndNavAsync()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO1.I2 != null && (x.BO1.I1 + x.BO1.I2) + x.BO1.I2 != null), await (await (Setters<TBO1_I, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async)))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotOrNavAsync()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO1.I2 != null || (x.BO1.I1 + x.BO1.I2) + x.BO1.I2 != null), await (await (Setters<TBO1_I, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async)))));
		}

		[Test]
		public async Task TestNestedPlusBehindOrNav2Async()
		{
			await (RunTestAsync(x => (x.BO1.I1 + x.BO1.I2) + x.BO1.I2 == null || (x.BO2.J1 + x.BO2.J2) + x.BO2.J2 == null, await (await (await (await (Setters<TBO1_I, TBO1_I, TBO2_J, TBO2_J>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async, MyBO.SetBO2_J1Async, MyBO.SetBO2_J2Async)))))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotOrNav2Async()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO1.I2 == null || (x.BO2.J1 + x.BO2.J2) + x.BO2.J2 == null), await (await (await (await (Setters<TBO1_I, TBO1_I, TBO2_J, TBO2_J>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async, MyBO.SetBO2_J1Async, MyBO.SetBO2_J2Async)))))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotAndNav2Async()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO1.I2 == null && (x.BO2.J1 + x.BO2.J2) + x.BO2.J2 == null), await (await (await (await (Setters<TBO1_I, TBO1_I, TBO2_J, TBO2_J>(MyBO.SetBO1_I1Async, MyBO.SetBO1_I2Async, MyBO.SetBO2_J1Async, MyBO.SetBO2_J2Async)))))));
		}

		[Test]
		public async Task TestNestedPlusBehindOrNav3Async()
		{
			await (RunTestAsync(x => (x.BO1.I1 + x.BO1.I2) + x.BO2.J2 == null || (x.BO2.J1 + x.BO2.J2) + x.BO1.I2 == null, await (await (await (await (Setters<TBO1_I, TBO2_J, TBO2_J, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO2_J2Async, MyBO.SetBO2_J1Async, MyBO.SetBO1_I2Async)))))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotOrNav3Async()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO2.J2 == null || (x.BO2.J1 + x.BO2.J2) + x.BO1.I2 == null), await (await (await (await (Setters<TBO1_I, TBO2_J, TBO2_J, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO2_J2Async, MyBO.SetBO2_J1Async, MyBO.SetBO1_I2Async)))))));
		}

		[Test]
		public async Task TestNestedPlusBehindNotAndNav3Async()
		{
			await (RunTestAsync(x => !((x.BO1.I1 + x.BO1.I2) + x.BO2.J2 == null && (x.BO2.J1 + x.BO2.J2) + x.BO1.I2 == null), await (await (await (await (Setters<TBO1_I, TBO2_J, TBO2_J, TBO1_I>(MyBO.SetBO1_I1Async, MyBO.SetBO2_J2Async, MyBO.SetBO2_J1Async, MyBO.SetBO1_I2Async)))))));
		}
	}
}
#endif
