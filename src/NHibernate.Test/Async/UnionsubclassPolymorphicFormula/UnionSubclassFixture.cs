#if NET_4_5
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.UnionsubclassPolymorphicFormula
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UnionSubclassFixture : TestCase
	{
		[Test, KnownBug("NH-2354")]
		public async Task QueryOverPersonTestAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					var person = new Person{FirstName = "Mark", LastName = "Mannson"};
					await (s.SaveAsync(person));
					var result = s.QueryOver<Party>().Where(p => p.Name == "Mark Mannson").SingleOrDefault();
					Assert.NotNull(result);
					await (s.DeleteAsync(result));
					await (t.CommitAsync());
				}
			}
		}

		[Test, KnownBug("NH-2354")]
		public async Task QueryOverCompanyTestAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					var company = new Company{CompanyName = "Limited", };
					await (s.SaveAsync(company));
					var result = s.QueryOver<Party>().Where(p => p.Name == "Limited").SingleOrDefault();
					Assert.NotNull(result);
				}
			}
		}
	}
}
#endif
