#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentEnumTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "PersistentEnum";
			}
		}

		private PersistentEnumClass p;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			p = new PersistentEnumClass(1, A.One, B.Two);
		}

		[Test]
		public async Task EqualsTrueAsync()
		{
			IType type = NHibernateUtil.Enum(typeof (A));
			A lhs = A.One;
			A rhs = A.One;
			Assert.IsTrue(await (type.IsEqualAsync(lhs, rhs, EntityMode.Poco)));
		}

		/// <summary>
		/// Verify that even if the Enum have the same underlying value but they
		/// are different Enums that they are not considered Equal.
		/// </summary>
		[Test]
		public async Task EqualsFalseSameUnderlyingValueAsync()
		{
			IType type = NHibernateUtil.Enum(typeof (A));
			A lhs = A.One;
			B rhs = B.One;
			Assert.IsFalse(await (type.IsEqualAsync(lhs, rhs, EntityMode.Poco)));
		}

		[Test]
		public async Task EqualsFalseAsync()
		{
			IType type = NHibernateUtil.Enum(typeof (A));
			A lhs = A.One;
			A rhs = A.Two;
			Assert.IsFalse(await (type.IsEqualAsync(lhs, rhs, EntityMode.Poco)));
		}

		[Test]
		public async Task UsageInHqlSelectNewAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(p));
				await (s.FlushAsync());
			}

			using (ISession s = sessions.OpenSession())
			{
				await (s.CreateQuery("select new PersistentEnumHolder(p.A, p.B) from PersistentEnumClass p").ListAsync());
				await (s.DeleteAsync("from PersistentEnumClass"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task UsageInHqlSelectNewInvalidConstructorAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(p));
				await (s.FlushAsync());
			}

			ISession s2 = sessions.OpenSession();
			try
			{
				Assert.ThrowsAsync<QueryException>(async () => await (s2.CreateQuery("select new PersistentEnumHolder(p.id, p.A, p.B) from PersistentEnumClass p").ListAsync()));
			}
			finally
			{
				await (s2.DeleteAsync("from PersistentEnumClass"));
				await (s2.FlushAsync());
				s2.Close();
			}
		}

		[Test]
		public async Task CanWriteAndReadUsingBothHeuristicAndExplicitGenericDeclarationAsync()
		{
			var persistentEnumClass = new PersistentEnumClass{Id = 1, A = A.Two, B = B.One};
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(persistentEnumClass));
				await (s.FlushAsync());
			}

			using (ISession s = sessions.OpenSession())
			{
				var saved = await (s.GetAsync<PersistentEnumClass>(1));
				Assert.That(saved.A, Is.EqualTo(A.Two));
				Assert.That(saved.B, Is.EqualTo(B.One));
				await (s.DeleteAsync(saved));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
