#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Test.NHSpecificTest.NH1845;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2893
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		[Explicit("Reproduces the issue only on Sybase SQL Anywhere with the driver configured with UseNamedPrefixInSql = false")]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var list = await (session.CreateCriteria<User>().Add(Restrictions.InsensitiveLike("Name", "Julian", MatchMode.Anywhere)).ListAsync<User>());
					Assert.That(list.Count, Is.EqualTo(1));
					Assert.That(list[0].Id, Is.EqualTo(1000));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
