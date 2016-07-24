#if NET_4_5
using System;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.UnionsubclassPolymorphicFormula
{
	[TestFixture, Explicit]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UnionSubclassFixtureAsync : TestCaseAsync
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
				return new string[]{"UnionsubclassPolymorphicFormula.Party.hbm.xml"};
			}
		}

		[Test]
		public async Task QueryOverPersonTestAsync()
		{
			Assert.ThrowsAsync<Exception>(async () =>
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var person = new Person{FirstName = "Mark", LastName = "Mannson"};
						await (s.SaveAsync(person));
						var result = await (s.QueryOver<Party>().Where(p => p.Name == "Mark Mannson").SingleOrDefaultAsync());
						Assert.NotNull(result);
						await (s.DeleteAsync(result));
						await (t.CommitAsync());
					}
				}
			}

			, KnownBug.Issue("NH-2354"));
		}

		[Test]
		public async Task QueryOverCompanyTestAsync()
		{
			Assert.ThrowsAsync<Exception>(async () =>
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var company = new Company{CompanyName = "Limited", };
						await (s.SaveAsync(company));
						var result = await (s.QueryOver<Party>().Where(p => p.Name == "Limited").SingleOrDefaultAsync());
						Assert.NotNull(result);
					}
				}
			}

			, KnownBug.Issue("NH-2354"));
		}
	}
}
#endif
