#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3609
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task AvgWithConditionalDoesNotThrowAsync()
		{
			try
			{
				AvgWithConditionalDoesNotThrow();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CountWithConditionalDoesNotThrowAsync()
		{
			try
			{
				CountWithConditionalDoesNotThrow();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task GroupByClauseHasParameterSetAsync()
		{
			try
			{
				GroupByClauseHasParameterSet();
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
