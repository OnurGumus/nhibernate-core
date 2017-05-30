﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.VersionTest.Db.MsSQL
{
	using System.Threading;
	[TestFixture]
	public class ComplexDomainFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new[] { "VersionTest.Db.MsSQL.ComplexVersioned.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}
		
		[Test]
		public async System.Threading.Tasks.Task NH1685Async()
		{
			using (ISession session = OpenSession())
			{
				var bar = new Bar {AField = 24};

				var foo = new Foo {AField = 42};
				foo.AddBar(bar);

				await (session.SaveAsync(foo, CancellationToken.None));
				await (session.FlushAsync(CancellationToken.None));
				await (session.EvictAsync(bar, CancellationToken.None));
				await (session.EvictAsync(foo, CancellationToken.None));

				var retrievedBar = await (session.GetAsync<Bar>(bar.Id, CancellationToken.None));

				// At this point the assumption is that bar and retrievedBar should have 
				// identical values, but represent two different POCOs. The asserts below 
				// are intended to verify this. Currently this test fails on the comparison 
				// of the SQL Server timestamp (i.e. binary(8)) fields because 
				// NHibernate does not retrieve the new timestamp after the last update. 

				Assert.AreNotSame(bar, retrievedBar);
				Assert.AreEqual(bar.Id, retrievedBar.Id);
				Assert.AreEqual(bar.AField, retrievedBar.AField);
				Assert.AreEqual(bar.Foo.Id, retrievedBar.Foo.Id);
				Assert.IsTrue(BinaryTimestamp.Equals(bar.Timestamp, retrievedBar.Timestamp), "Timestamps are different!");
			}

			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				await (session.DeleteAsync("from Bar", CancellationToken.None));
				await (session.Transaction.CommitAsync(CancellationToken.None));
			}
		}
	}
}