#if NET_4_5
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2530
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task WhenTryToGetHighThenExceptionShouldContainWhereClauseAsync()
		{
			try
			{
				WhenTryToGetHighThenExceptionShouldContainWhereClause();
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
