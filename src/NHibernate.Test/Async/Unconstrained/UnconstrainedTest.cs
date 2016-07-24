#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Unconstrained
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UnconstrainedTestAsync : UnconstrainedNoLazyTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Unconstrained.Person.hbm.xml"};
			}
		}
	}
}
#endif
