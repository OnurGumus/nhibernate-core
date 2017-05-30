﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Data;
using System.Data.Common;

using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

using NUnit.Framework;

namespace NHibernate.Test.TypeParameters
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// Test for parameterizable types.
	/// </summary>
	[TestFixture]
	public class TypeParameterTestAsync : TestCase
	{
		protected override IList Mappings
		{
			get
			{
				return new String[]
					{
						"TypeParameters.Typedef.hbm.xml",
						"TypeParameters.Widget.hbm.xml"
					};
			}
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		[Test]
		public async Task SaveAsync()
		{
			await (DeleteDataAsync(CancellationToken.None));

			ISession s = OpenSession();

			ITransaction t = s.BeginTransaction();

			Widget obj = new Widget();
			obj.ValueThree = 5;

			int id = (int) await (s.SaveAsync(obj, CancellationToken.None));

			await (t.CommitAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			t = s.BeginTransaction();

			IDriver driver = sessions.ConnectionProvider.Driver;

			var connection = s.Connection;
			var statement = driver.GenerateCommand(
				CommandType.Text,
				SqlString.Parse("SELECT * FROM STRANGE_TYPED_OBJECT WHERE ID=?"),
				new SqlType[] {SqlTypeFactory.Int32});
			statement.Connection = connection;
			t.Enlist(statement);
			statement.Parameters[0].Value = id;
			var reader = await (statement.ExecuteReaderAsync(CancellationToken.None));

			Assert.IsTrue(await (reader.ReadAsync(CancellationToken.None)), "A row should have been returned");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_ONE")) == DBNull.Value,
			              "Default value should have been mapped to null");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_TWO")) == DBNull.Value,
			              "Default value should have been mapped to null");
			Assert.AreEqual(Convert.ToInt32(reader.GetValue(reader.GetOrdinal("VALUE_THREE"))), 5,
			                "Non-Default value should not be changed");
			Assert.IsTrue(reader.GetValue(reader.GetOrdinal("VALUE_FOUR")) == DBNull.Value,
			              "Default value should have been mapped to null");
			reader.Close();

			await (t.CommitAsync(CancellationToken.None));
			s.Close();

            await (DeleteDataAsync(CancellationToken.None));
        }

		[Test]
		public async Task LoadingAsync()
		{
			await (InitDataAsync(CancellationToken.None));

			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			Widget obj = (Widget) await (s.CreateQuery("from Widget o where o.Str = :string")
			                      	.SetString("string", "all-normal").UniqueResultAsync(CancellationToken.None));
			Assert.AreEqual(obj.ValueOne, 7, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueTwo, 8, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueThree, 9, "Non-Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueFour, 10, "Non-Default value incorrectly loaded");

			obj = (Widget) await (s.CreateQuery("from Widget o where o.Str = :string")
			               	.SetString("string", "all-default").UniqueResultAsync(CancellationToken.None));
			Assert.AreEqual(obj.ValueOne, 1, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueTwo, 2, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueThree, -1, "Default value incorrectly loaded");
			Assert.AreEqual(obj.ValueFour, -5, "Default value incorrectly loaded");

			await (t.CommitAsync(CancellationToken.None));
			s.Close();

            await (DeleteDataAsync(CancellationToken.None));
        }

		private async Task InitDataAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			Widget obj = new Widget();
			obj.ValueOne = (7);
			obj.ValueTwo = (8);
			obj.ValueThree = (9);
			obj.ValueFour = (10);
			obj.Str = "all-normal";
			await (s.SaveAsync(obj, cancellationToken));

			obj = new Widget();
			obj.ValueOne = (1);
			obj.ValueTwo = (2);
			obj.ValueThree = (-1);
			obj.ValueFour = (-5);
			obj.Str = ("all-default");
			await (s.SaveAsync(obj, cancellationToken));

			await (t.CommitAsync(cancellationToken));
			s.Close();
		}

		private async Task DeleteDataAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.DeleteAsync("from Widget", cancellationToken));
			await (t.CommitAsync(cancellationToken));
			s.Close();
		}
	}
}