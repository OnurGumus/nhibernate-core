#if NET_4_5
using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Linq;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReflectionHelperIsMethodOfTests
	{
		[Test]
		public Task WhenNullMethodInfoThenThrowsAsync()
		{
			try
			{
				WhenNullMethodInfoThenThrows();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task WhenNullTypeThenThrowsAsync()
		{
			try
			{
				WhenNullTypeThenThrows();
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
