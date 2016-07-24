#if NET_4_5
using System;
using System.Collections;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdentityMapSequencedFixtureAsync : IdentityMapFixtureAsync
	{
		protected override IDictionary GetIdentityMap()
		{
			return IdentityMap.InstantiateSequenced(10);
		}
	}
}
#endif
