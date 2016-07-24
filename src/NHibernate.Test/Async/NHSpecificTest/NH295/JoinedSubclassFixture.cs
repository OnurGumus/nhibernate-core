#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH295
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinedSubclassFixtureAsync : SubclassFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH295.JoinedSubclass.hbm.xml"};
			}
		}
	}
}
#endif
