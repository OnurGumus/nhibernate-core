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
using NHibernate.Connection;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// Summary description for UserTypeFixture.
	/// </summary>
	[TestFixture]
	public class UserTypeFixtureAsync : TestCase
	{
		protected override IList Mappings
			=> new [] {"NHSpecific.ClassWithNullColumns.hbm.xml"};

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			{
				s.Delete("from ClassWithNullColumns");
				s.Flush();
			}
		}

		/// <summary>
		/// Does a quick test to make sure that a Property specified with a NullInt32UserType 
		/// persist to the db as a null.
		/// </summary>
		[Test]
		public async Task InsertNullAsync()
		{
			using (var s = OpenSession())
			{
				var userTypeClass = new ClassWithNullColumns
				{
					Id = 5,
					FirstInt32 = 4,
					SecondInt32 = 0
				};
				// with the user type should set 0 value to null

				await (s.SaveAsync(userTypeClass));
				await (s.FlushAsync());
			}

			// manually read from the db
			using (var provider = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				var conn = await (provider.GetConnectionAsync(CancellationToken.None));
				try
				{
					using (var cmd = conn.CreateCommand())
					{
						cmd.Connection = conn;
						cmd.CommandText = "select * from usertype";

						using (var reader = await (cmd.ExecuteReaderAsync(CancellationToken.None)))
						{
							var idOrdinal = reader.GetOrdinal("id");
							var firstOrdinal = reader.GetOrdinal("f_int32");
							var secondOrdinal = reader.GetOrdinal("s_int32");
							while (await (reader.ReadAsync(CancellationToken.None)))
							{
								Assert.AreEqual(5, reader[idOrdinal]);
								Assert.AreEqual(4, reader[firstOrdinal]);
								Assert.AreEqual(DBNull.Value, reader[secondOrdinal]);
								break;
							}
						}
					}
				}
				finally
				{
					provider.CloseConnection(conn);
				}
			}
		}
	}
}