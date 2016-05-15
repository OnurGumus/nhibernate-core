#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2703
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanOuterJoinMultipleTablesWithSimpleWithClauseAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Parent, Parent> query = session.QueryOver(() => RootElement);
				A A_Alias = null;
				B B_Alias = null;
				C C_Alias = null;
				query.Left.JoinQueryOver(parent => parent.C, () => C_Alias, c => c.PropC == A_Alias.PropA);
				query.Left.JoinQueryOver(parent => parent.A, () => A_Alias);
				query.Left.JoinQueryOver(parent => parent.B, () => B_Alias, b => b.PropB == C_Alias.PropC);
				// Expected join order: a --> c --> b
				// This query should not throw
				await (query.ListAsync());
			}
		}

		[Test]
		public async Task CanOuterJoinMultipleTablesWithComplexWithClauseAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Parent, Parent> query = session.QueryOver(() => RootElement);
				A A_Alias = null;
				B B_Alias = null;
				C C_Alias = null;
				query.Left.JoinQueryOver(parent => parent.C, () => C_Alias, c => c.PropC == A_Alias.PropA && c.PropC == B_Alias.PropB);
				query.Left.JoinQueryOver(parent => parent.A, () => A_Alias);
				query.Left.JoinQueryOver(parent => parent.B, () => B_Alias, b => b.PropB == A_Alias.PropA);
				// Expected join order: a --> b --> c
				// This query should not throw
				await (query.ListAsync());
			}
		}
	}
}
#endif
