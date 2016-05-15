#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3074
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		[Ignore("Fails on at least Oracle and PostgreSQL. See NH-3074 and NH-2408.")]
		public async Task HqlCanSetLockModeAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var cats = await (s.CreateQuery("select c from Animal c where c.Id=:id").SetInt32("id", Id).SetLockMode("c", LockMode.Upgrade).ListAsync<Cat>());
					Assert.That(cats, Is.Not.Empty);
				}
		}

		[Test, Ignore("Not fixed yet")]
		public async Task CritriaCanSetLockModeAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var cats = await (s.CreateCriteria<Animal>("c").Add(Restrictions.IdEq(Id)).SetLockMode("c", LockMode.Upgrade).ListAsync<Cat>());
					Assert.That(cats, Is.Not.Empty);
				}
		}
	}
}
#endif
