#if NET_4_5
using System.Data;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2302
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Cfg.Configuration configuration)
		{
			foreach (var cls in configuration.ClassMappings)
			{
				foreach (var prop in cls.PropertyIterator)
				{
					foreach (var col in prop.ColumnIterator)
					{
						if (col is Column)
						{
							var column = col as Column;
							if (column.SqlType == "nvarchar(max)")
								column.SqlType = Dialect.GetLongestTypeName(DbType.String);
						}
					}
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (CleanUpAsync());
			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task StringHugeLengthAsync()
		{
			int id;
			// buildup a string the exceed the mapping
			string str = GetFixedLengthString12000();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					// create and save the entity
					StringLengthEntity entity = new StringLengthEntity();
					entity.StringHugeLength = str;
					await (sess.SaveAsync(entity));
					await (tx.CommitAsync());
					id = entity.ID;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					StringLengthEntity loaded = await (sess.GetAsync<StringLengthEntity>(id));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(12000, loaded.StringHugeLength.Length);
					Assert.AreEqual(str, loaded.StringHugeLength);
					await (tx.CommitAsync());
				}
		}

		[Test, Ignore("Not supported without specify the string length.")]
		public async Task StringSqlTypeAsync()
		{
			int id;
			// buildup a string the exceed the mapping
			string str = GetFixedLengthString12000();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					// create and save the entity
					StringLengthEntity entity = new StringLengthEntity();
					entity.StringSqlType = str;
					await (sess.SaveAsync(entity));
					await (tx.CommitAsync());
					id = entity.ID;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					StringLengthEntity loaded = await (sess.GetAsync<StringLengthEntity>(id));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(12000, loaded.StringSqlType.Length);
					Assert.AreEqual(str, loaded.StringSqlType);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task BlobSqlTypeAsync()
		{
			int id;
			// buildup a string the exceed the mapping
			string str = GetFixedLengthString12000();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					// create and save the entity
					StringLengthEntity entity = new StringLengthEntity();
					entity.BlobSqlType = str;
					await (sess.SaveAsync(entity));
					await (tx.CommitAsync());
					id = entity.ID;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					StringLengthEntity loaded = await (sess.GetAsync<StringLengthEntity>(id));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(12000, loaded.BlobSqlType.Length);
					Assert.AreEqual(str, loaded.BlobSqlType);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task BlobWithLengthAsync()
		{
			int id;
			// buildup a string the exceed the mapping
			string str = GetFixedLengthString12000();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					// create and save the entity
					StringLengthEntity entity = new StringLengthEntity();
					entity.BlobLength = str;
					await (sess.SaveAsync(entity));
					await (tx.CommitAsync());
					id = entity.ID;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					StringLengthEntity loaded = await (sess.GetAsync<StringLengthEntity>(id));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(12000, loaded.BlobLength.Length);
					Assert.AreEqual(str, loaded.BlobLength);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task BlobWithoutLengthAsync()
		{
			int id;
			// buildup a string the exceed the mapping
			string str = GetFixedLengthString12000();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					// create and save the entity
					StringLengthEntity entity = new StringLengthEntity();
					entity.Blob = str;
					await (sess.SaveAsync(entity));
					await (tx.CommitAsync());
					id = entity.ID;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					StringLengthEntity loaded = await (sess.GetAsync<StringLengthEntity>(id));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(12000, loaded.Blob.Length);
					Assert.AreEqual(str, loaded.Blob);
					await (tx.CommitAsync());
				}
		}

		private async Task CleanUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from StringLengthEntity"));
					await (tx.CommitAsync());
				}
		}

		private static string GetFixedLengthString12000()
		{
			return new string ('a', 12000);
		}
	}
}
#endif
