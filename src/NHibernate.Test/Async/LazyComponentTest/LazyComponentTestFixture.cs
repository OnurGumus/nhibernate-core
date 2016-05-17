#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.LazyComponentTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyComponentTestFixture : TestCase
	{
		[Test]
		public async Task LazyLoadTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = s.CreateQuery("from Person p where name='Gabor'").UniqueResult<Person>();
					// make sure component has not been initialized yet
					Assert.That(NHibernateUtil.IsPropertyInitialized(p, "Address"), Is.False);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyDeleteTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = s.CreateQuery("from Person p where name='Gabor'").UniqueResult<Person>();
					// make sure component has not been initialized yet
					Assert.That(NHibernateUtil.IsPropertyInitialized(p, "Address"), Is.False);
					await (s.DeleteAsync(p));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task LazyUpdateTestAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = s.CreateQuery("from Person p where name='Gabor'").UniqueResult<Person>();
					// make sure component has not been initialized yet
					Assert.That(!NHibernateUtil.IsPropertyInitialized(p, "Address"));
					p.Address.City = "Baja";
					await (s.UpdateAsync(p));
					await (t.CommitAsync());
				}

			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var p = s.CreateQuery("from Person p where name='Gabor'").UniqueResult<Person>();
					Assert.That(p.Address.City, Is.EqualTo("Baja"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
