#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Util;
using System.Threading.Tasks;
using System;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BaseFixture : TestCase
	{
		public Task<string> GetSqlAsync(string query)
		{
			return GetSqlAsync(query, null);
		}

		public async Task<string> GetSqlAsync(string query, IDictionary<string, string> replacements)
		{
			var qt = new QueryTranslatorImpl(null, new HqlParseEngine(query, false, sessions).Parse(), emptyfilters, sessions);
			await (qt.CompileAsync(replacements, false));
			return qt.SQLString;
		}
	}
}
#endif
