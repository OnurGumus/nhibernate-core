#if NET_4_5
using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyRef1
	{
		public virtual async Task<MyRef2> GetOrCreateBO2Async(ISession s)
		{
			if (BO2 == null)
			{
				BO2 = new MyRef2();
				await (s.SaveAsync(BO2));
			}

			return BO2;
		}

		public virtual async Task<MyRef3> GetOrCreateBO3Async(ISession s)
		{
			if (BO3 == null)
			{
				BO3 = new MyRef3();
				await (s.SaveAsync(BO3));
			}

			return BO3;
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyBO
	{
		private async Task<MyRef1> GetOrCreateBO1Async(ISession s)
		{
			if (BO1 == null)
			{
				BO1 = new MyRef1();
				await (s.SaveAsync(BO1));
			}

			return BO1;
		}

		private async Task<MyRef2> GetOrCreateBO2Async(ISession s)
		{
			if (BO2 == null)
			{
				BO2 = new MyRef2();
				await (s.SaveAsync(BO2));
			}

			return BO2;
		}

		private static async Task SetBO1_IAsync(MyBO bo, ISession s, TBO1_I value, Action<MyRef1, int ? > set)
		{
			switch (value)
			{
				case TBO1_I.Null:
					bo.BO1 = null;
					break;
				case TBO1_I.ValueNull:
					set(await (bo.GetOrCreateBO1Async(s)), null);
					break;
				case TBO1_I.Zero:
					set(await (bo.GetOrCreateBO1Async(s)), 0);
					break;
				case TBO1_I.One:
					set(await (bo.GetOrCreateBO1Async(s)), 1);
					break;
				default:
					throw new Exception("Value " + value + " not handled in code");
			}
		}

		private static async Task SetBO2_JAsync(MyBO bo, ISession s, TBO2_J value, Action<MyRef2, int ? > set)
		{
			switch (value)
			{
				case TBO2_J.Null:
					bo.BO2 = null;
					break;
				case TBO2_J.ValueNull:
					set(await (bo.GetOrCreateBO2Async(s)), null);
					break;
				case TBO2_J.Zero:
					set(await (bo.GetOrCreateBO2Async(s)), 0);
					break;
				case TBO2_J.One:
					set(await (bo.GetOrCreateBO2Async(s)), 1);
					break;
				default:
					throw new Exception("Value " + value + " not handled in code");
			}
		}

		private static async Task SetBO1_BO2_JAsync(MyBO bo, ISession s, TBO1_BO2_J value, Action<MyRef2, int ? > set)
		{
			switch (value)
			{
				case TBO1_BO2_J.Null:
					bo.BO1 = null;
					break;
				case TBO1_BO2_J.BO1:
					(await (bo.GetOrCreateBO1Async(s))).BO2 = null;
					break;
				case TBO1_BO2_J.ValueNull:
					set(await ((await (bo.GetOrCreateBO1Async(s))).GetOrCreateBO2Async(s)), null);
					break;
				case TBO1_BO2_J.Zero:
					set(await ((await (bo.GetOrCreateBO1Async(s))).GetOrCreateBO2Async(s)), 0);
					break;
				case TBO1_BO2_J.One:
					set(await ((await (bo.GetOrCreateBO1Async(s))).GetOrCreateBO2Async(s)), 1);
					break;
				default:
					throw new Exception("Value " + value + " not handled in code");
			}
		}
	}
}
#endif
