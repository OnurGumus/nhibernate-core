#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

// ReSharper disable InconsistentNaming
namespace NHibernate.Test.NHSpecificTest.NH2705
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Test : BugTestCase
	{
		[Test]
		public Task HqlQueryWithFetch_WhenDerivedClassesUseComponentAndManyToOne_DoesNotGenerateInvalidSqlAsync()
		{
			try
			{
				HqlQueryWithFetch_WhenDerivedClassesUseComponentAndManyToOne_DoesNotGenerateInvalidSql();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task HqlQueryWithFetch_WhenDerivedClassesUseComponentAndEagerFetchManyToOne_DoesNotGenerateInvalidSqlAsync()
		{
			try
			{
				HqlQueryWithFetch_WhenDerivedClassesUseComponentAndEagerFetchManyToOne_DoesNotGenerateInvalidSql();
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
