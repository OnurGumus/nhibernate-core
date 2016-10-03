#if NET_4_5
using System;
using System.Collections;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ReadonlyTestCaseAsync : TestCaseAsync
	{
		protected override Task CreateSchemaAsync()
		{
			return TaskHelper.CompletedTask;
		}

		protected override Task DropSchemaAsync()
		{
			return TaskHelper.CompletedTask;
		}

		protected override Task<bool> CheckDatabaseWasCleanedAsync()
		{
			// We are read-only, so we're theoretically always clean.
			return Task.FromResult<bool>(true);
		}

		protected override void ApplyCacheSettings(Configuration configuration)
		{
		// Patrick Earl: I wasn't sure if making this do nothing was important, but I left it here since it wasn't running in the code when I changed it.
		}
	}
}
#endif
