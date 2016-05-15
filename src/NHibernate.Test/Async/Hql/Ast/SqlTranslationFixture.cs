#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlTranslationFixture : BaseFixture
	{
		[Test]
		public async Task ParseFloatConstantAsync()
		{
			const string query = "select 123.5, s from SimpleClass s";
			Assert.That(await (GetSqlAsync(query)), Is.StringStarting("select 123.5"));
		}

		[Test]
		public Task CaseClauseWithMathAsync()
		{
			try
			{
				CaseClauseWithMath();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task UnionAsync()
		{
			try
			{
				Union();
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
