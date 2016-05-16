using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.ElementsEnums
{
	[TestFixture]
	public partial class IntEnumsBagNoNameFixture : AbstractIntEnumsBagFixture
	{
		protected override IList Mappings
		{
			get { return new[] { "NHSpecificTest.ElementsEnums.SimpleWithEnumsNoName.hbm.xml" }; }
		}
	}
}