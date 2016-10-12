#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2477
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ConventionModelMapper();
			mapper.BeforeMapClass += (t, mi, map) => map.Id(idm => idm.Generator(Generators.Native));
			return mapper.CompileMappingFor(new[]{typeof (Something)});
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class Scenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public Scenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (var session = factory.OpenSession())
					using (session.BeginTransaction())
					{
						for (int i = 0; i < 5; i++)
						{
							session.Persist(new Something{Name = i.ToString()});
						}

						session.Transaction.Commit();
					}
			}

			public void Dispose()
			{
				using (var session = factory.OpenSession())
					using (session.BeginTransaction())
					{
						session.CreateQuery("delete from Something").ExecuteUpdate();
						session.Transaction.Commit();
					}
			}
		}

		[Test]
		public async Task WhenTakeBeforeCountShouldApplyTakeAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = sessions.OpenSession())
					using (session.BeginTransaction())
					{
						// This is another case where we have to work with subqueries and we have to write a specific query rewriter for Skip/Take instead flat the query in QueryReferenceExpressionFlattener
						//var actual = session.CreateQuery("select count(s) from Something s where s in (from Something take 3)").UniqueResult<long>();
						var actual = await (session.Query<Something>().Take(3).CountAsync());
						Assert.That(actual, Is.EqualTo(3));
					}
			}
		}
	}
}
#endif
