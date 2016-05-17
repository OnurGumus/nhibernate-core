#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3374
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
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
					NHibernateUtil.Initialize(blob.Bytes);
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
