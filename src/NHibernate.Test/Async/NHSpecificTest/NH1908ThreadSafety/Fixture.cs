﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate.Util;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1908ThreadSafety
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Dialect.Oracle8iDialect);
			// Oracle sometimes causes: ORA-12520: TNS:listener could not find available handler for requested type of server
			// Following links bizarrely suggest it's an Oracle limitation under load:
			// http://www.orafaq.com/forum/t/60019/2/ & http://www.ispirer.com/wiki/sqlways/troubleshooting-guide/oracle/import/tns_listener
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();

			if (!(Dialect is Dialect.FirebirdDialect))
				return;

			// Firebird will pool each connection created during the test and will not drop the created tables
			// which will result in other tests failing when they try to create tables with same name
			// By clearing the connection pool the tables will get dropped. This is done by the following code.
			var fbConnectionType = ReflectHelper.TypeFromAssembly("FirebirdSql.Data.FirebirdClient.FbConnection", "FirebirdSql.Data.FirebirdClient", false);
			var clearPool = fbConnectionType.GetMethod("ClearPool");
			var sillyConnection = Sfi.ConnectionProvider.GetConnection();
			clearPool.Invoke(null, new object[] { sillyConnection });
			Sfi.ConnectionProvider.CloseConnection(sillyConnection);
		}

		private async Task ScenarioRunningWithMultiThreadingAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var session = Sfi.OpenSession())
			{
				session
					.EnableFilter("CurrentOnly")
					.SetParameter("date", DateTime.Now);

				await (session.CreateQuery(
					@"
				select u
				from Order u
					left join fetch u.ActiveOrderLines
				where
					u.Email = :email
				")
					.SetString("email", "stupid@bugs.com")
					.UniqueResultAsync<Order>(cancellationToken));
			}
		}
	}
}
