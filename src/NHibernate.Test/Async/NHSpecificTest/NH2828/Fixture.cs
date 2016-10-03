#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2828
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task WhenPersistShouldNotFetchUninitializedCollectionAsync()
		{
			var companyId = await (CreateScenarioAsync());
			//Now in a second transaction i remove the address and persist Company: for a cascade option the Address will be removed
			using (var sl = new SqlLogSpy())
			{
				using (ISession session = sessions.OpenSession())
				{
					using (ITransaction tx = session.BeginTransaction())
					{
						var company = await (session.GetAsync<Company>(companyId));
						Assert.That(company.Addresses.Count(), Is.EqualTo(1));
						Assert.That(company.RemoveAddress(company.Addresses.First()), Is.EqualTo(true));
						//now this company will be saved and deleting the address.
						//BUT it should not try to load the BanckAccound collection!
						await (session.PersistAsync(company));
						await (tx.CommitAsync());
					}
				}

				var wholeMessage = sl.GetWholeLog();
				Assert.That(wholeMessage, Is.Not.StringContaining("BankAccount"));
			}

			await (CleanupAsync(companyId));
		}

		private async Task CleanupAsync(Guid companyId)
		{
			using (ISession session = sessions.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync(await (session.GetAsync<Company>(companyId))));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task<Guid> CreateScenarioAsync()
		{
			var company = new Company()
			{Name = "Company test"};
			var address = new Address()
			{Name = "Address test"};
			var bankAccount = new BankAccount()
			{Name = "Bank test"};
			company.AddAddress(address);
			company.AddBank(bankAccount);
			using (ISession session = sessions.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.PersistAsync(company));
					await (tx.CommitAsync());
				}
			}

			return company.Id;
		}
	}
}
#endif
