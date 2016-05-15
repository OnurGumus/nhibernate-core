#if NET_4_5
using System.Collections;
using NHibernate.Hql.Ast.ANTLR;
using System.Collections.Generic;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Test.BulkManipulation
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BaseFixture : TestCase
	{
		public async Task<string> GetSqlAsync(string query)
		{
			var qt = new QueryTranslatorImpl(null, new HqlParseEngine(query, false, sessions).Parse(), emptyfilters, sessions);
			await (qt.CompileAsync(null, false));
			return qt.SQLString;
		}
	}
}
#endif
