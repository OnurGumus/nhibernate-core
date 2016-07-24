#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestTestCaseAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		[Test]
		public async Task TestExecuteStatementAsync()
		{
			await (base.ExecuteStatementAsync("create table yyyy (x int)"));
			await (base.ExecuteStatementAsync("drop table yyyy"));
		}
	}
}
#endif
