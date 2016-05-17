#if NET_4_5
using System.Collections;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GetTest : TestCase
	{
		[Test]
		public async Task GetVsLoadAsync()
		{
			A a = new A("name");
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(a));
			}

			using (ISession s = OpenSession())
			{
				A loadedA = (A)s.Load(typeof (A), a.Id);
				Assert.IsFalse(NHibernateUtil.IsInitialized(loadedA), "Load should not initialize the object");
				Assert.IsNotNull(s.Load(typeof (A), (a.Id + 1)), "Loading non-existent object should not return null");
			}

			using (ISession s = OpenSession())
			{
				A gotA = (A)s.Get(typeof (A), a.Id);
				Assert.IsTrue(NHibernateUtil.IsInitialized(gotA), "Get should initialize the object");
				Assert.IsNull(s.Get(typeof (A), (a.Id + 1)), "Getting non-existent object should return null");
			}
		}

		[Test]
		public async Task GetAndModifyAsync()
		{
			A a = new A("name");
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(a));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				a = s.Get(typeof (A), a.Id) as A;
				a.Name = "modified";
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				a = s.Get(typeof (A), a.Id) as A;
				Assert.AreEqual("modified", a.Name, "the name was modified");
			}
		}

		[Test]
		public async Task GetAfterLoadAsync()
		{
			long id;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					A a = new A("name");
					id = (long)await (s.SaveAsync(a));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					A loadedA = (A)s.Load(typeof (A), id);
					Assert.IsFalse(NHibernateUtil.IsInitialized(loadedA));
					A gotA = (A)s.Get(typeof (A), id);
					Assert.IsTrue(NHibernateUtil.IsInitialized(gotA));
					Assert.AreSame(loadedA, gotA);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					A loadedNonExistentA = (A)s.Load(typeof (A), -id);
					Assert.IsFalse(NHibernateUtil.IsInitialized(loadedNonExistentA));
					// changed behavior because NH-1252
					Assert.IsNull(s.Get(typeof (A), -id));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
