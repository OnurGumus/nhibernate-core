#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2839
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCodeAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.Property(x => x.IsActive, pm => pm.Type<MyBooleanType>());
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob", IsActive = true};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally", IsActive = false};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
