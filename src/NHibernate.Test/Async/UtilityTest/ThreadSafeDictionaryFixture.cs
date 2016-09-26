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
	}
}
#endif
