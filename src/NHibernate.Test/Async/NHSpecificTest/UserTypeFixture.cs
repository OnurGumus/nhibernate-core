#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using NHibernate.Connection;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UserTypeFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecific.ClassWithNullColumns.hbm.xml"};
			}
		}

		/// <summary>
		/// Does a quick test to make sure that a Property specified with a NullInt32UserType 
		/// persist to the db as a null.
		/// </summary>
		[Test]
		public async Task InsertNullAsync()
		{
			using (ISession s = OpenSession())
			{
				ClassWithNullColumns userTypeClass = new ClassWithNullColumns();
				userTypeClass.Id = 5;
				userTypeClass.FirstInt32 = 4;
				userTypeClass.SecondInt32 = 0; // with the user type should set value to null
				await (s.SaveAsync(userTypeClass));
				await (s.FlushAsync());
			}

			// manually read from the db
			IConnectionProvider provider = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties);
			DbConnection conn = await (provider.GetConnectionAsync());
			DbCommand cmd = conn.CreateCommand();
			cmd.Connection = conn;
			cmd.CommandText = "select * from usertype";
			DbDataReader reader = await (cmd.ExecuteReaderAsync());
			while (await (reader.ReadAsync()))
			{
				Assert.AreEqual(5, reader[0]);
				Assert.AreEqual(4, reader[1]);
				Assert.AreEqual(DBNull.Value, reader[2]);
				break;
			}

			conn.Close();
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from ClassWithNullColumns"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
