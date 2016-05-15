#if NET_4_5
using System;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3138
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCode : TestCaseMappingByCode
	{
		[Test]
		public Task PageQueryWithDistinctAndOrderByContainingFunctionWithCommaSeparatedParametersAsync()
		{
			try
			{
				PageQueryWithDistinctAndOrderByContainingFunctionWithCommaSeparatedParameters();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		[Ignore("Failing")]
		public Task PageQueryWithDistinctAndOrderByContainingAliasedFunctionAsync()
		{
			try
			{
				PageQueryWithDistinctAndOrderByContainingAliasedFunction();
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
