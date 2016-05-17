#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LimitClauseFixture : BaseFixture
	{
		[Test]
		public async Task NoneAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h order by h.bodyWeight").List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{5, 6, 10, 15, 20};
			CollectionAssert.AreEqual(expected, actual);
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SkipAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h where h.bodyWeight > :minW order by h.bodyWeight skip 2").SetDouble("minW", 0d).List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{10, 15, 20};
			CollectionAssert.AreEqual(expected, actual);
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SkipTakeAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h order by h.bodyWeight skip 1 take 3").List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{6, 10, 15};
			CollectionAssert.AreEqual(expected, actual);
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SkipTakeWithParameterAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h order by h.bodyWeight skip :pSkip take :pTake").SetInt32("pSkip", 1).SetInt32("pTake", 3).List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{6f, 10f, 15f};
			Assert.That(actual, Is.EquivalentTo(expected));
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SkipTakeWithParameterListAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h where h.bodyWeight in (:list) order by h.bodyWeight skip :pSkip take :pTake").SetParameterList("list", new[]{10f, 15f, 5f}).SetInt32("pSkip", 1).SetInt32("pTake", 4).List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{10f, 15f};
			Assert.That(actual, Is.EquivalentTo(expected));
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SkipWithParameterAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h order by h.bodyWeight skip :jump").SetInt32("jump", 2).List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{10f, 15f, 20f};
			Assert.That(actual, Is.EquivalentTo(expected));
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TakeAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h order by h.bodyWeight take 2").List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{5, 6};
			CollectionAssert.AreEqual(expected, actual);
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TakeSkipAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			Assert.Throws<QuerySyntaxException>(() => s.CreateQuery("from Human h order by h.bodyWeight take 1 skip 2").List<Human>(), "take should not be allowed before skip");
			await (txn.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TakeWithParameterAsync()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();
			float[] actual = s.CreateQuery("from Human h where h.bodyWeight > :minW order by h.bodyWeight take :jump").SetDouble("minW", 1d).SetInt32("jump", 2).List<Human>().Select(h => h.BodyWeight).ToArray();
			var expected = new[]{5, 6};
			CollectionAssert.AreEqual(expected, actual);
			await (txn.CommitAsync());
			s.Close();
		}
	}
}
#endif
