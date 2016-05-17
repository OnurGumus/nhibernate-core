#if NET_4_5
using System;
using System.Collections;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.Test;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiPathCascadeTest : TestCase
	{
		[Test]
		public async Task MultiPathMergeModifiedDetachedAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			a = (A)s.Merge(a);
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
		}

		[Test]
		public async Task MultiPathMergeModifiedDetachedIntoProxyAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			A aLoaded = s.Load<A>(a.Id);
			Assert.That(aLoaded, Is.InstanceOf<INHibernateProxy>());
			Assert.That(s.Merge(a), Is.SameAs(aLoaded));
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
		}

		[Test]
		public async Task MultiPathUpdateModifiedDetachedAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			await (s.UpdateAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
		}

		[Test]
		public async Task MultiPathGetAndModifyAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			s = base.OpenSession();
			s.BeginTransaction();
			// retrieve the previously saved instance from the database, and update it
			a = await (s.GetAsync<A>(a.Id));
			this.ModifyEntity(a);
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
		}

		[Test]
		public async Task MultiPathMergeNonCascadedTransientEntityInCollectionAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			a = (A)s.Merge(a);
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
			// add a new (transient) G to collection in h
			// there is no cascade from H to the collection, so this should fail when merged
			Assert.That(a.Hs.Count, Is.EqualTo(1));
			H h = a.Hs.First();
			G gNew = new G();
			gNew.Data = "Gail";
			gNew.Hs.Add(h);
			h.Gs.Add(gNew);
			s = base.OpenSession();
			s.BeginTransaction();
			try
			{
				s.Merge(a);
				s.Merge(h);
				Assert.Fail("should have thrown TransientObjectException");
			}
			catch (TransientObjectException)
			{
			// expected
			}
			finally
			{
				s.Transaction.Rollback();
			}

			s.Close();
		}

		[Test]
		public async Task MultiPathMergeNonCascadedTransientEntityInOneToOneAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			a = (A)s.Merge(a);
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
			// change the one-to-one association from g to be a new (transient) A
			// there is no cascade from G to A, so this should fail when merged
			G g = a.G;
			a.G = null;
			A aNew = new A();
			aNew.Data = "Alice";
			g.A = aNew;
			aNew.G = g;
			s = base.OpenSession();
			s.BeginTransaction();
			try
			{
				s.Merge(a);
				s.Merge(g);
				Assert.Fail("should have thrown TransientObjectException");
			}
			catch (TransientObjectException)
			{
			// expected
			}
			finally
			{
				s.Transaction.Rollback();
			}

			s.Close();
		}

		[Test]
		public async Task MultiPathMergeNonCascadedTransientEntityInManyToOneAsync()
		{
			// persist a simple A in the database
			ISession s = base.OpenSession();
			s.BeginTransaction();
			A a = new A();
			a.Data = "Anna";
			await (s.SaveAsync(a));
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached entity
			this.ModifyEntity(a);
			s = base.OpenSession();
			s.BeginTransaction();
			a = (A)s.Merge(a);
			await (s.Transaction.CommitAsync());
			s.Close();
			await (this.VerifyModificationsAsync(a.Id));
			// change the many-to-one association from h to be a new (transient) A
			// there is no cascade from H to A, so this should fail when merged
			Assert.That(a.Hs.Count, Is.EqualTo(1));
			H h = a.Hs.First();
			a.Hs.Remove(h);
			A aNew = new A();
			aNew.Data = "Alice";
			aNew.AddH(h);
			s = base.OpenSession();
			s.BeginTransaction();
			try
			{
				s.Merge(a);
				s.Merge(h);
				Assert.Fail("should have thrown TransientObjectException");
			}
			catch (TransientObjectException)
			{
			// expected
			}
			finally
			{
				s.Transaction.Rollback();
			}

			s.Close();
		}

		private async Task VerifyModificationsAsync(long aId)
		{
			ISession s = base.OpenSession();
			s.BeginTransaction();
			// retrieve the A object and check it
			A a = await (s.GetAsync<A>(aId));
			Assert.That(a.Id, Is.EqualTo(aId));
			Assert.That(a.Data, Is.EqualTo("Anthony"));
			Assert.That(a.G, Is.Not.Null);
			Assert.That(a.Hs, Is.Not.Null);
			Assert.That(a.Hs.Count, Is.EqualTo(1));
			G gFromA = a.G;
			H hFromA = a.Hs.First();
			// check the G object
			Assert.That(gFromA.Data, Is.EqualTo("Giovanni"));
			Assert.That(gFromA.A, Is.SameAs(a));
			Assert.That(gFromA.Hs, Is.Not.Null);
			Assert.That(gFromA.Hs, Is.EqualTo(a.Hs));
			Assert.That(gFromA.Hs.First(), Is.SameAs(hFromA));
			// check the H object
			Assert.That(hFromA.Data, Is.EqualTo("Hellen"));
			Assert.That(hFromA.A, Is.SameAs(a));
			Assert.That(hFromA.Gs, Is.Not.Null);
			Assert.That(hFromA.Gs.Count, Is.EqualTo(1));
			Assert.That(hFromA.Gs.First(), Is.SameAs(gFromA));
			await (s.Transaction.CommitAsync());
			s.Close();
		}
	}
}
#endif
