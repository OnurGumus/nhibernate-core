#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.FileStreamSql2008
{
	[TestFixture, Explicit]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.FileStreamSql2008.Mappings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2008Dialect;
		}

		protected override void Configure(Configuration cfg)
		{
			//Points to the database created with FileStream Filegroup.
			cfg.Properties["connection.connection_string"] = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=FileStreamDB;Integrated Security=True;Pooling=False";
#region CREATE DATABASE example
		/*
			CREATE DATABASE FileStreamDB ON PRIMARY
			  ( NAME = FileStreamDB_data,
				FILENAME = N'C:\FSDemo\FileStreamDB_data.mdf',
				SIZE = 10MB,
				MAXSIZE = 50MB,
				FILEGROWTH = 10%),
			FILEGROUP RowGroup1
			  ( NAME = FileStreamDB_group1,
				FILENAME = N'C:\FSDemo\FileStreamDB_group1.ndf',
				SIZE = 10MB,
				MAXSIZE = 50MB,
				FILEGROWTH = 5MB),
			FILEGROUP FileStreamGroup1 CONTAINS FILESTREAM
			  ( NAME = FileStreamDBResumes,
				FILENAME = N'C:\FSDemo\VendorCatalog')
			LOG ON
			  ( NAME = 'FileStreamDB_log',
				FILENAME = N'C:\FSDemo\FileStreamDB_log.ldf',
				SIZE = 5MB,
				MAXSIZE = 25MB,
				FILEGROWTH = 5MB);
*/
#endregion
		}

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
