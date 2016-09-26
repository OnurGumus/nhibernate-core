#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CharEqualityTestsAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(ca =>
			{
				ca.Id(x => x.Id, map => map.Generator(Generators.Assigned));
				ca.Property(x => x.Name, map => map.Length(150));
				ca.Property(x => x.Type, map => map.Length(1));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Id = 1000, Name = "Person Type A", Type = 'A'}));
					await (session.SaveAsync(new Person{Id = 1001, Name = "Person Type B", Type = 'B'}));
					await (session.SaveAsync(new Person{Id = 1002, Name = "Person Type C", Type = 'C'}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (transaction.CommitAsync());
				}
		}

		private IList<Person> Execute(Func<IStatelessSession, IQueryable<Person>> query)
		{
			using (var session = Sfi.OpenStatelessSession())
				using (session.BeginTransaction())
				{
					return query(session).ToList();
				}
		}
	}
}
#endif
