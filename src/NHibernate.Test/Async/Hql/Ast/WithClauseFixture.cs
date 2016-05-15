#if NET_4_5
using System.Collections;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WithClauseFixture : BaseFixture
	{
		[Test]
		public async Task WithClauseFailsWithFetchAsync()
		{
			var data = new TestData(this);
			await (data.PrepareAsync());
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			Assert.Throws<SemanticException>(() => s.CreateQuery("from Animal a inner join fetch a.offspring as o with o.bodyWeight = :someLimit").SetDouble("someLimit", 1).List(), "ad-hoc on clause allowed with fetched association");
			await (txn.CommitAsync());
			s.Close();
			await (data.CleanupAsync());
		}

		[Test]
		public async Task InvalidWithSemanticsAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			// PROBLEM : f.bodyWeight is a reference to a column on the Animal table; however, the 'f'
			// alias relates to the Human.friends collection which the aonther Human entity.  The issue
			// here is the way JoinSequence and Joinable (the persister) interact to generate the
			// joins relating to the sublcass/superclass tables
			Assert.Throws<InvalidWithClauseException>(() => s.CreateQuery("from Human h inner join h.friends as f with f.bodyWeight < :someLimit").SetDouble("someLimit", 1).List());
			Assert.Throws<InvalidWithClauseException>(() => s.CreateQuery("from Animal a inner join a.offspring o inner join o.mother as m inner join m.father as f with o.bodyWeight > 1").List());
			Assert.Throws<InvalidWithClauseException>(() => s.CreateQuery("from Human h inner join h.offspring o with o.mother.father = :cousin").SetEntity("cousin", s.Load<Human>(123L)).List());
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task WithClauseAsync()
		{
			var data = new TestData(this);
			await (data.PrepareAsync());
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			// one-to-many
			IList list = await (s.CreateQuery("from Human h inner join h.offspring as o with o.bodyWeight < :someLimit").SetDouble("someLimit", 1).ListAsync());
			Assert.That(list, Is.Empty, "ad-hoc on did not take effect");
			// many-to-one
			list = await (s.CreateQuery("from Animal a inner join a.mother as m with m.bodyWeight < :someLimit").SetDouble("someLimit", 1).ListAsync());
			Assert.That(list, Is.Empty, "ad-hoc on did not take effect");
			// many-to-many
			list = await (s.CreateQuery("from Human h inner join h.friends as f with f.nickName like 'bubba'").ListAsync());
			Assert.That(list, Is.Empty, "ad-hoc on did not take effect");
			await (txn.CommitAsync());
			s.Close();
			await (data.CleanupAsync());
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestData
		{
			public async Task PrepareAsync()
			{
				ISession session = tc.OpenNewSession();
				ITransaction txn = session.BeginTransaction();
				var mother = new Human{BodyWeight = 10, Description = "mother"};
				var father = new Human{BodyWeight = 15, Description = "father"};
				var child1 = new Human{BodyWeight = 5, Description = "child1"};
				var child2 = new Human{BodyWeight = 6, Description = "child2"};
				var friend = new Human{BodyWeight = 20, Description = "friend"};
				child1.Mother = mother;
				child1.Father = father;
				mother.AddOffspring(child1);
				father.AddOffspring(child1);
				child2.Mother = mother;
				child2.Father = father;
				mother.AddOffspring(child2);
				father.AddOffspring(child2);
				father.Friends = new[]{friend};
				await (session.SaveAsync(mother));
				await (session.SaveAsync(father));
				await (session.SaveAsync(child1));
				await (session.SaveAsync(child2));
				await (session.SaveAsync(friend));
				await (txn.CommitAsync());
				session.Close();
			}

			public async Task CleanupAsync()
			{
				ISession session = tc.OpenNewSession();
				ITransaction txn = session.BeginTransaction();
				await (session.CreateQuery("delete Animal where mother is not null").ExecuteUpdateAsync());
				IList humansWithFriends = await (session.CreateQuery("from Human h where exists(from h.friends)").ListAsync());
				foreach (var friend in humansWithFriends)
				{
					await (session.DeleteAsync(friend));
				}

				await (session.CreateQuery("delete Animal").ExecuteUpdateAsync());
				await (txn.CommitAsync());
				session.Close();
			}
		}
	}
}
#endif
