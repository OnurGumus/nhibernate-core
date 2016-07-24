#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Test.NHSpecificTest.NH1845;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2893
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<User>(rc =>
			{
				rc.Id(x => x.Id);
				rc.Property(x => x.Name, mapping => mapping.Length(256));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return (dialect is Dialect.SybaseSQLAnywhere12Dialect);
		}

		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
			{
				configuration.SetProperty("hbm2ddl.keywords", "auto-quote");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var newUser = new User()
					{Id = 1000, Name = "Julian Maughan"};
					await (session.SaveAsync(newUser));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from User").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

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
