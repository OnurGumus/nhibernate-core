#if NET_4_5
using System;
using System.Collections;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2166
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		[Test]
		public async Task WhenUniqueResultShouldCallConverterAsync()
		{
			using (var s = OpenSession())
			{
				Assert.That(async () => await (s.CreateSQLQuery("select make from ItFunky").UniqueResultAsync<int>()), Throws.TypeOf<GenericADOException>());
			}
		}
	}
}
#endif
