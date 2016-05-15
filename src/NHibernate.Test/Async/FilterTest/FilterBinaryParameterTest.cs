#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.FilterTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterBinaryParameterTest : TestCase
	{
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
