#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2905
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task JoinOverMultipleSteps_MethodSyntax_SelectAndSelectManyAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity1>().Select(x => x.Entity2).SelectMany(x => x.Entity3s).Where(x => x.Id == _entity3Id).ToList();
					await (tx.CommitAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					Assert.That(result[0].Id, Is.EqualTo(_entity3Id));
				}
		}

		[Test]
		public async Task JoinOverMultipleSteps_MethodSyntax_OnlySelectManyAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity1>().SelectMany(x => x.Entity2.Entity3s).Where(x => x.Id == _entity3Id).ToList();
					await (tx.CommitAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					Assert.That(result[0].Id, Is.EqualTo(_entity3Id));
				}
		}

		[Test]
		public async Task JoinOverMultipleSteps_QuerySyntax_LetAndFromAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = (
						from e1 in session.Query<Entity1>()let e2 = e1.Entity2
						from e3 in e2.Entity3s
						where e3.Id == _entity3Id
						select e3).ToList();
					await (tx.CommitAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					Assert.That(result[0].Id, Is.EqualTo(_entity3Id));
				}
		}

		[Test]
		public async Task JoinOverMultipleSteps_QuerySyntax_OnlyFromAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = (
						from e1 in session.Query<Entity1>()from e3 in e1.Entity2.Entity3s
						where e3.Id == _entity3Id
						select e3).ToList();
					await (tx.CommitAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					Assert.That(result[0].Id, Is.EqualTo(_entity3Id));
				}
		}
	}
}
#endif
