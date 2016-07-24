#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Subclass
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubclassExtendsFixtureAsync : SubclassFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				// order is important!  The base classes must be configured before
				// the subclasses.
				return new string[]{"Subclass.Subclass.Base.hbm.xml", "Subclass.Subclass.One.hbm.xml"};
			}
		}
	}
}
#endif
