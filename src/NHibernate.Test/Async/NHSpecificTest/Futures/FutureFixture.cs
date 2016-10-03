#if NET_4_5
using System.Collections;
using NHibernate.Driver;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FutureFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.Futures.Mappings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected void IgnoreThisTestIfMultipleQueriesArentSupportedByDriver()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (driver.SupportsMultipleQueries == false)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
		}
	}
}
#endif
