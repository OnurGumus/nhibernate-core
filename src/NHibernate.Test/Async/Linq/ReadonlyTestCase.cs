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
	public abstract partial class ReadonlyTestCase : TestCase
	{
		protected override Task CreateSchemaAsync()
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
