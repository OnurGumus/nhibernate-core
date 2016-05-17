#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using NExp = NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH47Fixture : TestCase
	{
		public async Task<TimeSpan> BatchInsertAsync(object[] objs)
		{
			TimeSpan tspan = TimeSpan.Zero;
			if (objs != null && objs.Length > 0)
			{
				ISession s = OpenSession();
				ITransaction t = s.BeginTransaction();
				int count = objs.Length;
				Console.WriteLine();
				Console.WriteLine("Start batch insert " + count.ToString() + " objects");
				DateTime startTime = DateTime.Now;
				for (int i = 0; i < count; ++i)
				{
					await (s.SaveAsync(objs[i]));
				}

				await (t.CommitAsync());
				s.Close();
				tspan = DateTime.Now.Subtract(startTime);
				Console.WriteLine("Finish in " + tspan.TotalMilliseconds.ToString() + " milliseconds");
			}

			return tspan;
		}

		[Test, Explicit]
		public async Task TestNH47Async()
		{
			int testCount = 100;
			object[] al = new object[testCount];
			TimeSpan tspan = TimeSpan.Zero;
			int times = 1000;
			for (int i = 0; i < times; ++i)
			{
				for (int j = 0; j < testCount; ++j)
				{
					UnsavedType ut = new UnsavedType();
					ut.Id = j + 1 + testCount * (i + 1);
					ut.TypeName = Guid.NewGuid().ToString();
					al[j] = ut;
				}

				tspan = tspan.Add(await (BatchInsertAsync(al)));
			}

			Console.WriteLine("Finish average in " + (tspan.TotalMilliseconds / times).ToString() + " milliseconds for " + times.ToString() + " times");
			Console.Read();
		}
	}
}
#endif
