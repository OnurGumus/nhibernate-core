#if NET_4_5
using System;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2505
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		private Regex caseClause = new Regex("case", RegexOptions.IgnoreCase);
		protected override HbmMapping GetMappings()
		{
			var mapper = new ConventionModelMapper();
			mapper.BeforeMapClass += (mi, t, x) => x.Id(map => map.Generator(Generators.Guid));
			return mapper.CompileMappingFor(new[]{typeof (MyClass)});
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class Scenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public Scenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (var session = factory.OpenSession())
				{
					session.Save(new MyClass{Alive = true});
					session.Save(new MyClass{Alive = false, MayBeAlive = true});
					session.Flush();
				}
			}

			public void Dispose()
			{
				using (var session = factory.OpenSession())
				{
					session.CreateQuery("delete from MyClass").ExecuteUpdate();
					session.Flush();
				}
			}
		}

		[Test]
		public async Task WhenQueryConstantEqualToMemberThenDoesNotUseTheCaseConstructorAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						var list = await (session.Query<MyClass>().Where(x => x.Alive == false).ToListAsync());
						Assert.That(list, Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(0));
					}

					using (var sqls = new SqlLogSpy())
					{
						var list = await (session.Query<MyClass>().Where(x => true == x.Alive).ToListAsync());
						Assert.That(list, Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(0));
					}
				}
			}
		}

		[Test]
		public async Task WhenQueryConstantNotEqualToMemberThenDoesNotUseTheCaseConstructorAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => x.Alive != false).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(0));
					}

					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => true != x.Alive).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(0));
					}
				}
			}
		}

		[Test]
		public async Task WhenQueryComplexEqualToComplexThentUseTheCaseConstructorForBothAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						await (session.Query<MyClass>().Where(x => (5 > x.Something) == (x.Something < 10)).ToListAsync());
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(2));
					}
				}
			}
		}

		[Test]
		public async Task WhenQueryConstantEqualToNullableMemberThenUseTheCaseConstructorForMemberAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => x.MayBeAlive == false).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}

					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => true == x.MayBeAlive).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}
				}
			}
		}

		[Test]
		public async Task WhenQueryConstantEqualToNullableMemberValueThenDoesNotUseTheCaseConstructorForMemberAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						await (session.Query<MyClass>().Where(x => x.MayBeAlive.Value == false).ToListAsync());
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}

					using (var sqls = new SqlLogSpy())
					{
						await (session.Query<MyClass>().Where(x => true == x.MayBeAlive.Value).ToListAsync());
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}
				}
			}
		}

		[Test]
		public async Task WhenQueryConstantNotEqualToNullableMemberThenUseTheCaseConstructorForMemberAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => x.MayBeAlive != false).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}

					using (var sqls = new SqlLogSpy())
					{
						Assert.That(await (session.Query<MyClass>().Where(x => true != x.MayBeAlive).ToListAsync()), Has.Count.EqualTo(1));
						Assert.That(caseClause.Matches(sqls.GetWholeLog()).Count, Is.EqualTo(1));
					}
				}
			}
		}
	}
}
#endif
