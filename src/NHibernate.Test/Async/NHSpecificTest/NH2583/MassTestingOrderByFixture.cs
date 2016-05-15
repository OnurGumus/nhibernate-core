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
	public partial class MassTestingOrderByFixture : AbstractMassTestingFixture
	{
		// Condition pattern: (A && B) && (C || D) ORDER BY F
		[Test]
		public async Task Test_xyP_in_F____xy_OJAsync()
		{
			await (RunTestAsync(x => (x.K1 == 1 && x.K1 == 1) && (x.K2 == 1 || x.K3 == 1), // The last setter forces MyBOs to have a pointer to a BO1 so that we can check
			// that the right BO1.Ids are returned by the Select(bo => (int?) bo.BO1.Id)!
			await (Setters<TK, TK, TK, TBO1_I>(MyBO.SetK1, MyBO.SetK2, MyBO.SetK3, MyBO.SetBO1_I1Async))));
		}

		[Test]
		public async Task Test_xy_in_A__xyP_in_F____xy_OJAsync()
		{
			await (RunTestAsync(x => (x.BO1.I1 == 1 && x.K1 == 1) && (x.K2 == 1 || x.K3 == 1), // Here, SetBO1_I1 is already called to populate the value for the expression -
			// therefore, an additional call to force BO1 objects is not necessary.
			await (Setters<TBO1_I, TK, TK, TK>(MyBO.SetBO1_I1Async, MyBO.SetK1, MyBO.SetK2, MyBO.SetK3))));
		}

		[Test]
		public async Task Test_xyP_in_A__xyP_in_F____xy_IJAsync()
		{
			await (RunTestAsync(x => (x.BO1.I1 == 1 && x.BO1.Id > 0 && x.K1 == 1) && (x.K2 == 1 || x.K3 == 1), await (Setters<TBO1_I, TK, TK, TK>(MyBO.SetBO1_I1Async, MyBO.SetK1, MyBO.SetK2, MyBO.SetK3))));
		}

		[Test]
		public async Task Test_xyP_in_A_C_F____xy_IJAsync()
		{
			await (RunTestAsync(x => (x.BO1.I1 == 1 && x.BO1.Id > 0 && x.K1 == 1) && (x.BO1.Id > 0 && x.K2 == 1 || x.K3 == 1), await (Setters<TBO1_I, TK, TK, TK>(MyBO.SetBO1_I1Async, MyBO.SetK1, MyBO.SetK2, MyBO.SetK3))));
		}

		[Test]
		public async Task Test_xyP_in_A_C_D_F____xy_IJAsync()
		{
			await (RunTestAsync(x => (x.BO1.I1 == 1 && x.BO1.Id > 0 && x.K1 == 1) && (x.BO1.Id > 0 && x.K2 == 1 || x.BO1.Id > 0 && x.K3 == 1), await (Setters<TBO1_I, TK, TK, TK>(MyBO.SetBO1_I1Async, MyBO.SetK1, MyBO.SetK2, MyBO.SetK3))));
		}

		[Test]
		public async Task Test_xyP_in_C_D_F____xy_IJAsync()
		{
			await (RunTestAsync(x => (x.K1 == 1 && x.K1 == 1) && (x.BO1.Id > 0 && x.K2 == 1 || x.BO1.Id > 0 && x.K3 == 1), await (Setters<TK, TK, TBO1_I, TK>(MyBO.SetK1, MyBO.SetK2, MyBO.SetBO1_I1Async, MyBO.SetK3))));
		}

		[Test]
		public async Task Test_xyP_in_C_F____xy_OJAsync()
		{
			await (RunTestAsync(x => (x.K1 == 1 && x.K1 == 1) && (x.BO1.Id > 0 && x.K2 == 1 || x.K3 == 1), await (Setters<TK, TK, TBO1_I, TK>(MyBO.SetK1, MyBO.SetK2, MyBO.SetBO1_I1Async, MyBO.SetK3))));
		}
	}
}
#endif
