#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.SqlConverterAndMultiQuery
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task NormalHqlShouldThrowUserExceptionAsync()
		{
			try
			{
				NormalHqlShouldThrowUserException();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task MultiHqlShouldThrowUserExceptionAsync()
		{
			try
			{
				MultiHqlShouldThrowUserException();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task NormalCriteriaShouldThrowUserExceptionAsync()
		{
			try
			{
				NormalCriteriaShouldThrowUserException();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task MultiCriteriaShouldThrowUserExceptionAsync()
		{
			try
			{
				MultiCriteriaShouldThrowUserException();
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
