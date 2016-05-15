#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypeParameters
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TypeParameterTest : TestCase
	{
		[Test]
		public async Task SaveAsync()
		{
			await (DeleteDataAsync());
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Widget obj = new Widget();
			obj.ValueThree = 5;
			int id = (int)await (s.SaveAsync(obj));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IDriver driver = sessions.ConnectionProvider.Driver;
			DbConnection connection = s.Connection;
			DbCommand statement = driver.GenerateCommand(CommandType.Text, SqlString.Parse("SELECT * FROM STRANGE_TYPED_OBJECT WHERE ID=?"), new SqlType[]{SqlTypeFactory.Int32});
			statement.Connection = connection;
			t.Enlist(statement);
			((DbParameter)statement.Parameters[0]).Value = id;
			DbDataReader reader = await (statement.ExecuteReaderAsync());
			Assert.IsTrue(await (reader.ReadAsync()), "A row should have been returned");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_ONE")) == DBNull.Value, "Default value should have been mapped to null");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_TWO")) == DBNull.Value, "Default value should have been mapped to null");
			Assert.AreEqual(Convert.ToInt32(reader.GetValue(reader.GetOrdinal("VALUE_THREE"))), 5, "Non-Default value should not be changed");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_FOUR")) == DBNull.Value, "Default value should have been mapped to null");
			reader.Close();
			await (t.CommitAsync());
			s.Close();
			await (DeleteDataAsync());
		}

		[Test]
		public async Task LoadingAsync()
		{
			await (InitDataAsync());
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Widget obj = (Widget)await (s.CreateQuery("from Widget o where o.Str = :string").SetString("string", "all-normal").UniqueResultAsync());
			Assert.AreEqual(obj.ValueOne, 7, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueTwo, 8, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueThree, 9, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueFour, 10, "Non-Default value incorrectly loaded");
			obj = (Widget)await (s.CreateQuery("from Widget o where o.Str = :string").SetString("string", "all-default").UniqueResultAsync());
			Assert.AreEqual(obj.ValueOne, 1, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueTwo, 2, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueThree, -1, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueFour, -5, "Default value incorrectly loaded");
			await (t.CommitAsync());
			s.Close();
			await (DeleteDataAsync());
		}

		private async Task InitDataAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Widget obj = new Widget();
			obj.ValueOne = (7);
			obj.ValueTwo = (8);
			obj.ValueThree = (9);
			obj.ValueFour = (10);
			obj.Str = "all-normal";
			await (s.SaveAsync(obj));
			obj = new Widget();
			obj.ValueOne = (1);
			obj.ValueTwo = (2);
			obj.ValueThree = (-1);
			obj.ValueFour = (-5);
			obj.Str = ("all-default");
			await (s.SaveAsync(obj));
			await (t.CommitAsync());
			s.Close();
		}

		private async Task DeleteDataAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.DeleteAsync("from Widget"));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
