#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.VersionTest.Db.MsSQL
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyVersionTestAsync : TestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"VersionTest.Db.MsSQL.ProductWithVersionAndLazyProperty.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test(Description = "NH-3589")]
		public async System.Threading.Tasks.Task CanUseVersionOnEntityWithLazyPropertyAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.SaveAsync(new ProductWithVersionAndLazyProperty{Id = 1, Summary = "Testing, 1, 2, 3"}));
					await (session.FlushAsync());
					session.Clear();
					var p = await (session.GetAsync<ProductWithVersionAndLazyProperty>(1));
					p.Summary += ", 4!";
					await (session.FlushAsync());
				}
		}
	}
}
#endif
