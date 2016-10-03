#if NET_4_5
using System;
using log4net;
using log4net.Repository.Hierarchy;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TypeFactoryFixtureAsync
	{
		public TypeFactoryFixtureAsync()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		private static readonly ILog log = LogManager.GetLogger(typeof (TypeFactoryFixtureAsync));
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class GenericPropertyClass
		{
			private long ? _genericLong;
			public long ? GenericInt64
			{
				get
				{
					return _genericLong;
				}

				set
				{
					_genericLong = value;
				}
			}
		}

		private readonly Random rnd = new Random();
		private int totalCall;
		public enum MyEnum
		{
			One
		}
	}
}
#endif
