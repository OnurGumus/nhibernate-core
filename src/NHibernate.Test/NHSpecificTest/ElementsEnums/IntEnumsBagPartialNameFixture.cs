using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.ElementsEnums
{
	[TestFixture]
	public partial class IntEnumsBagPartialNameFixture : AbstractIntEnumsBagFixture
	{
		protected override IList Mappings
		{
			get { return new[] { "NHSpecificTest.ElementsEnums.SimpleWithEnumsPartialName.hbm.xml" }; }
		}
	}
}