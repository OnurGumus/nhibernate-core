#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Util;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BaseFixtureAsync : TestCaseAsync
	{
		private readonly IDictionary<string, IFilter> emptyfilters = new CollectionHelper.EmptyMapClass<string, IFilter>();
#region Overrides of TestCase
		protected override IList Mappings
		{
			get
			{
				return new string[0];
			}
		}

#endregion
		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
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

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

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
