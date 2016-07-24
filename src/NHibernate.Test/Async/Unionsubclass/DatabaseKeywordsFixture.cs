#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.Unionsubclass
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DatabaseKeywordsFixtureAsync : TestCaseAsync
	{
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
				return new string[]{"Unionsubclass.DatabaseKeyword.hbm.xml"};
			}
		}

		protected override async Task ConfigureAsync(Configuration configuration)
		{
			await (base.ConfigureAsync(configuration));
			configuration.SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote");
		}

		[Test]
		public async Task UnionSubClassQuotesReservedColumnNamesAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new DatabaseKeyword()
					{User = "user", View = "view", Table = "table", Create = "create"}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from DatabaseKeywordBase"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
