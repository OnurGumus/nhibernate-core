#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3564
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCodeAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.Property(x => x.DateOfBirth, pm =>
				{
					pm.Type(NHibernateUtil.Timestamp);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.SetProperty(Environment.CacheProvider, typeof (MyDummyCacheProvider).AssemblyQualifiedName);
				configuration.SetProperty(Environment.UseQueryCache, "true");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Name = "Bob", DateOfBirth = new DateTime(2015, 4, 22)}));
					await (session.SaveAsync(new Person{Name = "Sally", DateOfBirth = new DateTime(2014, 4, 22)}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void ShouldUseDifferentCache()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var bob = session.Query<Person>().Cacheable().Where(e => e.DateOfBirth == new DateTime(2015, 4, 22)).ToList();
					var sally = session.Query<Person>().Cacheable().Where(e => e.DateOfBirth == new DateTime(2014, 4, 22)).ToList();
					Assert.That(bob, Has.Count.EqualTo(1));
					Assert.That(bob[0].Name, Is.EqualTo("Bob"));
					Assert.That(sally, Has.Count.EqualTo(1));
					Assert.That(sally[0].Name, Is.EqualTo("Sally"));
				}
		}
	}
}
#endif
