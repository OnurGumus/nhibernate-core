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
	// related issues NH-1687, NH-1685

	[TestFixture]
	public class GeneratedBinaryVersionFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new[] { "VersionTest.Db.MsSQL.SimpleVersioned.hbm.xml" }; }
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
		public async System.Threading.Tasks.Task ShouldRetrieveVersionAfterFlushAsync()
		{
			// Note : if you are using identity-style strategy the value of version
			// is available inmediately after save.
			var e = new SimpleVersioned {Something = "something"};
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Assert.That(e.LastModified, Is.Null);
					await (s.SaveAsync(e, CancellationToken.None));
					await (s.FlushAsync(CancellationToken.None));
					Assert.That(e.LastModified, Is.Not.Null);
					await (s.DeleteAsync(e, CancellationToken.None));
					await (tx.CommitAsync(CancellationToken.None));
				}
			}
		}

		[Test]
		public async System.Threading.Tasks.Task ShouldChangeAfterUpdateAsync()
		{
			object savedId = await (PersistANewSomethingAsync(CancellationToken.None));
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var fetched = await (s.GetAsync<SimpleVersioned>(savedId, CancellationToken.None));
					var freshVersion = fetched.LastModified;
					fetched.Something = "make it dirty";
					await (s.UpdateAsync(fetched, CancellationToken.None));
					await (s.FlushAsync(CancellationToken.None)); // force flush to hit DB
					Assert.That(fetched.LastModified, Is.Not.SameAs(freshVersion));
					await (s.DeleteAsync(fetched, CancellationToken.None));
					await (tx.CommitAsync(CancellationToken.None));
				}
			}
		}

		private async System.Threading.Tasks.Task<object> PersistANewSomethingAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			object savedId;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var e = new SimpleVersioned {Something = "something"};
					savedId = await (s.SaveAsync(e, cancellationToken));
					await (tx.CommitAsync(cancellationToken));
				}
			}
			return savedId;
		}

		[Test]
		public async System.Threading.Tasks.Task ShouldCheckStaleStateAsync()
		{
			var versioned = new SimpleVersioned {Something = "original string"};

			try
			{
				using (var session = OpenSession())
				{
					await (session.SaveAsync(versioned, CancellationToken.None));
					await (session.FlushAsync(CancellationToken.None));

					using (var concurrentSession = OpenSession())
					{
						var sameVersioned = await (concurrentSession.GetAsync<SimpleVersioned>(versioned.Id, CancellationToken.None));
						sameVersioned.Something = "another string";
						await (concurrentSession.FlushAsync(CancellationToken.None));
					}

					versioned.Something = "new string";

					var expectedException = sessions.Settings.IsBatchVersionedDataEnabled
						? Throws.InstanceOf<StaleStateException>()
						: Throws.InstanceOf<StaleObjectStateException>();

					Assert.That(() => session.Flush(), expectedException);
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync("from SimpleVersioned", CancellationToken.None));
					await (session.FlushAsync(CancellationToken.None));
				}
			}
		}
	}
}