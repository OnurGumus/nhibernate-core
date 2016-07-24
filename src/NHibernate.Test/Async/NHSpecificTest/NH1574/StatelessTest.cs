#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1574
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task StatelessManyToOneAsync()
		{
			using (ISession session = OpenSession())
			{
				var principal = new SpecializedPrincipal();
				var team = new SpecializedTeamStorage();
				principal.Team = team;
				await (session.SaveOrUpdateAsync(team));
				await (session.SaveOrUpdateAsync(principal));
				await (session.FlushAsync());
			}

			using (IStatelessSession session = sessions.OpenStatelessSession())
			{
				IQuery query = session.CreateQuery("from SpecializedPrincipal p");
				IList<Principal> principals = await (query.ListAsync<Principal>());
				Assert.AreEqual(1, principals.Count);
				ITransaction trans = session.BeginTransaction();
				foreach (var principal in principals)
				{
					principal.Name = "Buu";
					await (session.UpdateAsync(principal));
				}

				await (trans.CommitAsync());
			}

			// cleanup
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from SpecializedTeamStorage"));
				await (session.DeleteAsync("from SpecializedPrincipal"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
