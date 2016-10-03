#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3374
{
	[Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Document>(rc =>
			{
				rc.Id(x => x.Id, idMapper => idMapper.Generator(Generators.Identity));
				rc.ManyToOne(x => x.Blob, m =>
				{
					m.Cascade(Mapping.ByCode.Cascade.All);
				}

				);
				rc.Property(x => x.Name);
			}

			);
			mapper.Class<Blob>(map =>
			{
				map.Id(x => x.Id, idMapper => idMapper.Generator(Generators.Identity));
				map.Property(x => x.Bytes, y =>
				{
					y.Column(x =>
					{
						x.SqlType("varbinary(max)");
						x.Length(int.MaxValue);
					}

					);
					y.Lazy(true);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Document{Name = "Bob"};
					e1.Blob = new Blob{Bytes = new byte[]{1, 2, 3}};
					await (session.SaveAsync(e1));
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

		[Test]
		public async Task TestNoTargetExceptionAsync()
		{
			Document document = await (LoadDetachedEntityAsync());
			Blob blob = await (LoadDetachedBlobAsync());
			blob.Bytes = new byte[]{4, 5, 6};
			document.Blob = blob;
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					session.Merge(document);
				}
		}

		private async Task<Blob> LoadDetachedBlobAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var blob = await (session.GetAsync<Blob>(1));
					await (NHibernateUtil.InitializeAsync(blob.Bytes));
					return blob;
				}
		}

		private async Task<Document> LoadDetachedEntityAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					return await (session.GetAsync<Document>(1));
				}
		}
	}
}
#endif
