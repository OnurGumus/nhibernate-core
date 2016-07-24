#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.JoinedSubclass
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinedSubclassExtendsFixtureAsync : JoinedSubclassFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				// order is important!  The base classes must be configured before
				// the subclasses.
				return new string[]{"JoinedSubclass.JoinedSubclass.Person.hbm.xml", "JoinedSubclass.JoinedSubclass.Employee.hbm.xml", "JoinedSubclass.JoinedSubclass.Customer.hbm.xml"};
			}
		}
	}
}
#endif
