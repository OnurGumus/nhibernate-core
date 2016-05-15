#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2580
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task WhenPersisterNotFoundShouldThrowAMoreExplicitExceptionAsync()
		{
			try
			{
				WhenPersisterNotFoundShouldThrowAMoreExplicitException();
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
