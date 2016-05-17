#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.VersionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class VersionFixture : TestCase
	{
		[Test]
		public async System.Threading.Tasks.Task VersionShortCircuitFlushAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Person gavin = new Person("Gavin");
			new Thing("Passport", gavin);
			await (s.SaveAsync(gavin));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Thing passp = (Thing)s.Get(typeof (Thing), "Passport");
			passp.LongDescription = "blah blah blah";
			s.CreateQuery("from Person").List();
			s.CreateQuery("from Person").List();
			s.CreateQuery("from Person").List();
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(passp.Version, 2);
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync("from Thing"));
			await (s.DeleteAsync("from Person"));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async System.Threading.Tasks.Task CollectionVersionAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Person gavin = new Person("Gavin");
			new Thing("Passport", gavin);
			await (s.SaveAsync(gavin));
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, gavin.Version);
			s = OpenSession();
			t = s.BeginTransaction();
			gavin = (Person)await (s.CreateCriteria(typeof (Person)).UniqueResultAsync());
			new Thing("Laptop", gavin);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(2, gavin.Version);
			Assert.IsFalse(NHibernateUtil.IsInitialized(gavin.Things));
			s = OpenSession();
			t = s.BeginTransaction();
			gavin = (Person)await (s.CreateCriteria(typeof (Person)).UniqueResultAsync());
			gavin.Things.Clear();
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(3, gavin.Version);
			Assert.IsTrue(NHibernateUtil.IsInitialized(gavin.Things));
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(gavin));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async System.Threading.Tasks.Task CollectionNoVersionAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Person gavin = new Person("Gavin");
			new Task("Code", gavin);
			await (s.SaveAsync(gavin));
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, gavin.Version);
			s = OpenSession();
			t = s.BeginTransaction();
			gavin = (Person)await (s.CreateCriteria(typeof (Person)).UniqueResultAsync());
			new Task("Document", gavin);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, gavin.Version);
			Assert.IsFalse(NHibernateUtil.IsInitialized(gavin.Tasks));
			s = OpenSession();
			t = s.BeginTransaction();
			gavin = (Person)await (s.CreateCriteria(typeof (Person)).UniqueResultAsync());
			gavin.Tasks.Clear();
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, gavin.Version);
			Assert.IsTrue(NHibernateUtil.IsInitialized(gavin.Tasks));
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(gavin));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
