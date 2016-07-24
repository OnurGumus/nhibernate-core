#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NHibernate.Dialect;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2030
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void GetTypeWithLenShouldBeThreadSafe()
		{
			object sync = new object ();
			List<Exception> exceptions = new List<Exception>();
			ManualResetEvent startEvent = new ManualResetEvent(false);
			var action = new ThreadStart(() =>
			{
				startEvent.WaitOne();
				try
				{
					for (int i = 0; i < 1000; i++)
					{
						SqlTypeFactory.GetString(i);
					}
				}
				catch (Exception e)
				{
					lock (sync)
					{
						exceptions.Add(e);
					}
				}
			}

			);
			const int threadCount = 30;
			Thread[] threads = new Thread[threadCount];
			for (int i = 0; i < threadCount; i++)
			{
				threads[i] = new Thread(action);
				threads[i].Start();
			}

			startEvent.Set();
			foreach (var thread in threads)
			{
				thread.Join();
			}

			if (exceptions.Count > 0)
			{
				foreach (var e in exceptions)
					Console.WriteLine(e);
				throw exceptions[0];
			}
		}
	}
}
#endif
