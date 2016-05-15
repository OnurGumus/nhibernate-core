#if NET_4_5
using System.Collections;
using NUnit.Framework;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Test.Interceptor
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InterceptorFixture : TestCase
	{
		[Test]
		public async Task CollectionInterceptAsync()
		{
			ISession s = OpenSession(new CollectionInterceptor());
			ITransaction t = s.BeginTransaction();
			User u = new User("Gavin", "nivag");
			await (s.PersistAsync(u));
			u.Password = "vagni";
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			u = await (s.GetAsync<User>("Gavin"));
			Assert.AreEqual(2, u.Actions.Count);
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task PropertyInterceptAsync()
		{
			ISession s = OpenSession(new PropertyInterceptor());
			ITransaction t = s.BeginTransaction();
			User u = new User("Gavin", "nivag");
			await (s.PersistAsync(u));
			u.Password = "vagni";
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			u = await (s.GetAsync<User>("Gavin"));
			Assert.IsTrue(u.Created.HasValue);
			Assert.IsTrue(u.LastUpdated.HasValue);
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
		}

		///
		///Here the interceptor resets the
		///current-state to the same thing as the current db state; this
		///causes EntityPersister.FindDirty() to return no dirty properties.
		///
		[Test]
		public async Task PropertyIntercept2Async()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			User u = new User("Josh", "test");
			await (s.PersistAsync(u));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession(new HHH1921Interceptor());
			t = s.BeginTransaction();
			u = await (s.GetAsync<User>(u.Name));
			u.Password = "nottest";
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			u = await (s.GetAsync<User>("Josh"));
			Assert.AreEqual("test", u.Password);
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ComponentInterceptorAsync()
		{
			const int checkPerm = 500;
			const string checkComment = "generated from interceptor";
			ISession s = OpenSession(new MyComponentInterceptor(checkPerm, checkComment));
			ITransaction t = s.BeginTransaction();
			Image i = new Image();
			i.Name = "compincomp";
			i = (Image)await (s.MergeAsync(i));
			Assert.IsNotNull(i.Details);
			Assert.AreEqual(checkPerm, i.Details.Perm1);
			Assert.AreEqual(checkComment, i.Details.Comment);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			i = await (s.GetAsync<Image>(i.Id));
			Assert.IsNotNull(i.Details);
			Assert.AreEqual(checkPerm, i.Details.Perm1);
			Assert.AreEqual(checkComment, i.Details.Comment);
			await (s.DeleteAsync(i));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task StatefulInterceptAsync()
		{
			StatefulInterceptor statefulInterceptor = new StatefulInterceptor();
			ISession s = OpenSession(statefulInterceptor);
			Assert.IsNotNull(statefulInterceptor.Session);
			ITransaction t = s.BeginTransaction();
			User u = new User("Gavin", "nivag");
			await (s.PersistAsync(u));
			u.Password = "vagni";
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IList logs = await (s.CreateCriteria(typeof (Log)).ListAsync());
			Assert.AreEqual(2, logs.Count);
			await (s.DeleteAsync(u));
			await (s.DeleteAsync("from Log"));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
