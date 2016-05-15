#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using NHibernate.Hql.Ast.ANTLR;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedParametersFixture : TestCase
	{
		[Test]
		public Task TestMissingHQLParametersAsync()
		{
			try
			{
				TestMissingHQLParameters();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
