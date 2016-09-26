#if NET_4_5
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2931
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MappingByCodeTestAsync : BugTestCaseAsync
	{
		//no xml mappings here, since we use MappingByCode
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[0];
			}
		}
	}
}
#endif
