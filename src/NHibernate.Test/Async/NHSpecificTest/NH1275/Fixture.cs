﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1275
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// http://nhibernate.jira.com/browse/NH-1275
	/// </summary>
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH1275"; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !string.IsNullOrEmpty(dialect.ForUpdateString);
		}

		[Test]
		public async Task RetrievingAsync()
		{
			object savedId;
			using(ISession s = OpenSession())
			using(ITransaction t = s.BeginTransaction())
			{
				A a  = new A("hunabKu");
				savedId = await (s.SaveAsync(a, CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
				{
					await (s.GetAsync<A>(savedId, LockMode.Upgrade, CancellationToken.None));
					string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
					Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
				}
				using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
				{
					await (s.CreateQuery("from A a where a.Id= :pid").SetLockMode("a", LockMode.Upgrade).SetParameter("pid", savedId).
							UniqueResultAsync<A>(CancellationToken.None));
					string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
					Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
				}
				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from A", CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task LokingAsync()
		{
			object savedId;
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				A a = new A("hunabKu");
				savedId = await (s.SaveAsync(a, CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				A a = await (s.GetAsync<A>(savedId, CancellationToken.None));
				using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
				{
					await (s.LockAsync(a, LockMode.Upgrade, CancellationToken.None));
					string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
					Assert.Less(0, sql.IndexOf(Dialect.ForUpdateString));
				}
				await (t.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from A", CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}
		}
	}
}
