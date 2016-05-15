#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Naturalid.Immutable
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ImmutableNaturalIdFixture : TestCase
	{
		[Test]
		public async Task UpdateAsync()
		{
			// prepare some test data...
			User user;
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				user = new User();
				user.UserName = "steve";
				user.Email = "steve@hibernate.org";
				user.Password = "brewhaha";
				await (session.SaveAsync(user));
				await (session.Transaction.CommitAsync());
			}

			// 'user' is now a detached entity, so lets change a property and reattch...
			user.Password = "homebrew";
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				await (session.UpdateAsync(user));
				await (session.Transaction.CommitAsync());
			}

			// clean up
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				await (session.DeleteAsync(user));
				await (session.Transaction.CommitAsync());
			}
		}

		[Test]
		public async Task NaturalIdCheckAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			User u = new User("steve", "superSecret");
			await (s.PersistAsync(u));
			u.UserName = "Steve";
			try
			{
				await (s.FlushAsync());
				Assert.Fail();
			}
			catch (HibernateException)
			{
			}

			u.UserName = "steve";
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task NaturalIdCacheAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			User u = new User("steve", "superSecret");
			await (s.PersistAsync(u));
			await (s.Transaction.CommitAsync());
			s.Close();
			sessions.Statistics.Clear();
			s = OpenSession();
			s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			await (s.Transaction.CommitAsync());
			s.Close();
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCachePutCount);
			s = OpenSession();
			s.BeginTransaction();
			User v = new User("gavin", "supsup");
			await (s.PersistAsync(v));
			await (s.Transaction.CommitAsync());
			s.Close();
			sessions.Statistics.Clear();
			s = OpenSession();
			s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(0, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCacheHitCount);
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(0, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(2, sessions.Statistics.QueryCacheHitCount);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			await (s.DeleteAsync("from User"));
			await (s.Transaction.CommitAsync());
			s.Close();
		}
	}
}
#endif
