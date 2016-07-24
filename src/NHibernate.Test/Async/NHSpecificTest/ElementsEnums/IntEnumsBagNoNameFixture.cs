#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.ElementsEnums
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IntEnumsBagNoNameFixtureAsync : AbstractIntEnumsBagFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.ElementsEnums.SimpleWithEnumsNoName.hbm.xml"};
			}
		}
	}
}
#endif
