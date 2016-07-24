#if NET_4_5
using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ThreadSafeDictionaryFixtureAsync
	{
		public ThreadSafeDictionaryFixtureAsync()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		private static readonly ILog log = LogManager.GetLogger(typeof (ThreadSafeDictionaryFixtureAsync));
		private readonly Random rnd = new Random();
		private int read, write;
		[Test, Explicit]
		public void MultiThreadAccess()
		{
			MultiThreadRunner<IDictionary<int, int>>.ExecuteAction[] actions = new MultiThreadRunner<IDictionary<int, int>>.ExecuteAction[]{delegate (IDictionary<int, int> d)
			{
				try
				{
					log.DebugFormat("T{0} Add", Thread.CurrentThread.Name);
					write++;
					d.Add(rnd.Next(), rnd.Next());
				}
				catch (ArgumentException)
				{
				// duplicated key
				}
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} ContainsKey", Thread.CurrentThread.Name);
				read++;
				d.ContainsKey(rnd.Next());
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} Remove", Thread.CurrentThread.Name);
				write++;
				d.Remove(rnd.Next());
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} TryGetValue", Thread.CurrentThread.Name);
				read++;
				int val;
				d.TryGetValue(rnd.Next(), out val);
			}

			, delegate (IDictionary<int, int> d)
			{
				try
				{
					log.DebugFormat("T{0} get_this[]", Thread.CurrentThread.Name);
					read++;
					int val = d[rnd.Next()];
				}
				catch (KeyNotFoundException)
				{
				// not foud key
				}
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} set_this[]", Thread.CurrentThread.Name);
				write++;
				d[rnd.Next()] = rnd.Next();
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} Keys", Thread.CurrentThread.Name);
				read++;
				IEnumerable<int> e = d.Keys;
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} Values", Thread.CurrentThread.Name);
				read++;
				IEnumerable<int> e = d.Values;
			}

			, delegate (IDictionary<int, int> d)
			{
				log.DebugFormat("T{0} GetEnumerator", Thread.CurrentThread.Name);
				read++;
				foreach (KeyValuePair<int, int> pair in d)
				{
				}
			}

			, };
			MultiThreadRunner<IDictionary<int, int>> mtr = new MultiThreadRunner<IDictionary<int, int>>(20, actions);
			IDictionary<int, int> wrapper = new ThreadSafeDictionary<int, int>(new Dictionary<int, int>());
			mtr.EndTimeout = 2000;
			mtr.Run(wrapper);
			log.DebugFormat("{0} reads, {1} writes -- elements {2}", read, write, wrapper.Count);
		}
	}
}
#endif
