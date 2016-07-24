#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH645
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HqlFunctionWithAstHqlParserAsync : HQLFunctionFixtureBaseAsync
	{
		protected override async Task ConfigureAsync(Configuration configuration)
		{
			await (base.ConfigureAsync(configuration));
			configuration.SetProperty(Environment.QueryTranslator, typeof (ASTQueryTranslatorFactory).AssemblyQualifiedName);
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class HQLFunctionFixtureBaseAsync : TestCaseAsync
	{
		private bool appliesToThisDialect = true;
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Hql.Animal.hbm.xml", "Hql.MaterialResource.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return appliesToThisDialect;
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				if (Dialect is MsSql2005Dialect)
					configuration.SetProperty(Environment.Dialect, typeof (CustomDialect).AssemblyQualifiedName);
				else
					appliesToThisDialect = false;
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public async Task RunAsync(string hql)
		{
			using (ISession s = OpenSession())
				try
				{
					await (s.CreateQuery(hql).ListAsync());
				}
				catch (Exception ex)
				{
					if (ex.GetType().FullName == "Antlr.Runtime.Tree.RewriteEmptyStreamException" || ex is InvalidCastException)
						Assert.Fail("The parser think that 'freetext' is a boolean function");
				}
		}

		/// <summary>
		/// Just test the parser can compile, and SqlException is expected.
		/// </summary>
		[Test]
		public async Task SimpleWhereAsync()
		{
			await (RunAsync("from Animal a where freetext(a.Description, 'hey apple car')"));
		}

		[Test]
		public async Task SimpleWhereWithAnotherClauseAsync()
		{
			await (RunAsync("from Animal a where freetext(a.Description, 'hey apple car') AND 1 = 1"));
		}

		[Test]
		public async Task SimpleWhereWithAnotherClause2Async()
		{
			await (RunAsync("from Animal a where freetext(a.Description, 'hey apple car') AND a.Description <> 'foo'"));
		}
	}
}
#endif
