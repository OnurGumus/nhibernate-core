#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1421
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task WhenParameterListIsEmptyUsingQueryThenDoesNotTrowsNullReferenceExceptionAsync()
		{
			try
			{
				WhenParameterListIsEmptyUsingQueryThenDoesNotTrowsNullReferenceException();
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
