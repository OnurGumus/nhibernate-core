#if NET_4_5
using System.Text.RegularExpressions;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1821
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task ShouldNotRemoveLineBreaksFromSqlQueriesAsync()
		{
			try
			{
				ShouldNotRemoveLineBreaksFromSqlQueries();
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
