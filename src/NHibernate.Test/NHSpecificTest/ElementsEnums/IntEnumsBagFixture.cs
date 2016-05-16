using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.ElementsEnums
{
	[TestFixture]
	public partial class IntEnumsBagFixture : AbstractIntEnumsBagFixture
	{
		protected override IList Mappings
		{
			get { return new[] { "NHSpecificTest.ElementsEnums.SimpleWithEnums.hbm.xml" }; }
		}
	}
}