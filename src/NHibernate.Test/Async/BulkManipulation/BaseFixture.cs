#if NET_4_5
using System.Collections;
using NHibernate.Hql.Ast.ANTLR;
using System.Collections.Generic;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Test.BulkManipulation
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BaseFixtureAsync : TestCaseAsync
	{
		private readonly IDictionary<string, IFilter> emptyfilters = new CollectionHelper.EmptyMapClass<string, IFilter>();
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

		protected override void Configure(Cfg.Configuration configuration)
		{
			var assembly = GetType().Assembly;
			string mappingNamespace = GetType().Namespace;
			foreach (var resource in assembly.GetManifestResourceNames())
			{
				if (resource.StartsWith(mappingNamespace) && resource.EndsWith(".hbm.xml"))
				{
					configuration.AddResource(resource, assembly);
				}
			}
		}

		public async Task<string> GetSqlAsync(string query)
		{
			var qt = new QueryTranslatorImpl(null, new HqlParseEngine(query, false, sessions).Parse(), emptyfilters, sessions);
			await (qt.CompileAsync(null, false));
			return qt.SQLString;
		}
	}
}
#endif
