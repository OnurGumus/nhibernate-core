using System;
using System.Collections;
using NHibernate.Util;
using NUnit.Framework;

namespace NHibernate.Test.UtilityTest
{
	/// <summary>
	/// Tests a Sequenced Identity Map.
	/// </summary>
	[TestFixture]
	public partial class IdentityMapSequencedFixture : IdentityMapFixture
	{
		protected override IDictionary GetIdentityMap()
		{
			return IdentityMap.InstantiateSequenced(10);
		}
	}
}