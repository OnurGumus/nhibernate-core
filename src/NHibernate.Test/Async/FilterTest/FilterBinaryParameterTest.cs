#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.FilterTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterBinaryParameterTestAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"FilterTest.BinaryFiltered.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test]
		public async Task NH882Async()
		{
			using (ISession session = OpenSession())
			{
				byte[] binValue = {0xFF, 0xFF, 0xFF};
				session.EnableFilter("BinaryFilter").SetParameter("BinaryValue", binValue);
				IQuery query = session.CreateQuery("from BinaryFiltered");
				await (query.ListAsync());
			}
		}
	}
}
#endif
