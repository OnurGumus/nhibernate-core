#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1574
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessTest : BugTestCase
	{
		[Test]
		public async Task StatelessManyToOneAsync()
		{
			using (ISession session = OpenSession())
			{
				var principal = new SpecializedPrincipal();
				var team = new SpecializedTeamStorage();
				principal.Team = team;
				session.SaveOrUpdate(team);
				session.SaveOrUpdate(principal);
				await (session.FlushAsync());
			}

			using (IStatelessSession session = sessions.OpenStatelessSession())
			{
				IQuery query = session.CreateQuery("from SpecializedPrincipal p");
				IList<Principal> principals = query.List<Principal>();
				Assert.AreEqual(1, principals.Count);
				ITransaction trans = session.BeginTransaction();
				foreach (var principal in principals)
				{
					principal.Name = "Buu";
					session.Update(principal);
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
