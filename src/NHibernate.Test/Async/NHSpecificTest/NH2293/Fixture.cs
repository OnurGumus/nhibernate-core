#if NET_4_5
using System.Linq;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2293
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task WhenQueryHasJustAfromThenThrowQuerySyntaxExceptionAsync()
		{
			try
			{
				WhenQueryHasJustAfromThenThrowQuerySyntaxException();
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
