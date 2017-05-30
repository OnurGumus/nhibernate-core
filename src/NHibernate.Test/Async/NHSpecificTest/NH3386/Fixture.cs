﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using NHibernate.SqlCommand;

namespace NHibernate.Test.NHSpecificTest.NH3386
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.MsSql2000Dialect;
		}

		protected override void OnSetUp()
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				var e1 = new Entity {Name = "Bob"};
				session.Save(e1);

				var e2 = new Entity {Name = "Sally"};
				session.Save(e2);

				session.Flush();
				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete("from System.Object");

				session.Flush();
				transaction.Commit();
			}
		}

		[Test]
		public void ShouldSupportNonRuntimeExtensionWithoutEntityReferenceAsync()
		{
			var sqlInterceptor = new SqlInterceptor();
			using (ISession session = OpenSession(sqlInterceptor))
			using (session.BeginTransaction())
			{
				var result = session.Query<Entity>()
					.OrderBy(e => SqlServerFunction.NewID());

				Assert.DoesNotThrowAsync(() => { return result.ToListAsync(CancellationToken.None); });
				Assert.That(sqlInterceptor.Sql.ToString(), Does.Contain(nameof(SqlServerFunction.NewID)).IgnoreCase);
			}
		}
	}
}