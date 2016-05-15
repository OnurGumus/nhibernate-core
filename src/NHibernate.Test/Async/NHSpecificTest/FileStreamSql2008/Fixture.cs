#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.FileStreamSql2008
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task SavingAndRetrievingAsync()
		{
			Guid rowId = Guid.NewGuid();
			var entity = new VendorCatalog{Name = "Dario", CatalogID = rowId, Catalog = Convert.ToBytes("Aqui me pongo a cantar...al compas de la viguela")};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			VendorCatalog entityReturned = null;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					entityReturned = await (s.CreateQuery("from VendorCatalog").UniqueResultAsync<VendorCatalog>());
					Assert.AreEqual("Dario", entityReturned.Name);
					Assert.AreEqual(rowId.ToString(), entityReturned.CatalogID.ToString());
					Assert.AreEqual("Aqui me pongo a cantar...al compas de la viguela", Convert.ToStr(entityReturned.Catalog));
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(entityReturned));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
