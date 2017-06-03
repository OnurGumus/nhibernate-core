﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Test.Classic
{
	using System.Threading.Tasks;
	[TestFixture]
	public class LifecycleFixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new[] { "Classic.EntityWithLifecycle.hbm.xml" }; }
		}

		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.GenerateStatistics, "true");
		}

		[Test]
		public async Task SaveAsync()
		{
			Sfi.Statistics.Clear();
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(new EntityWithLifecycle()));
				await (s.FlushAsync());
			}
			Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(0));

			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task UpdateAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}

			// update detached
			Sfi.Statistics.Clear();
			v.Heigth = 0;
			using (ISession s = OpenSession())
			{
				await (s.UpdateAsync(v));
				await (s.FlushAsync());
			}
			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(0));

			// cleanup
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdateAsync());
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task MergeAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}
			v.Heigth = 0;
			Sfi.Statistics.Clear();
			using (ISession s = OpenSession())
			{
				s.Merge(v);
				await (s.FlushAsync());
			}
			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(0));

			var v1 = new EntityWithLifecycle("Shinobi", 0, 10);
			using (ISession s = OpenSession())
			{
				s.Merge(v1);
				await (s.FlushAsync());
			}
			Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(0));
			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(0));


			// cleanup
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdateAsync());
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task DeleteAsync()
		{
			var v = new EntityWithLifecycle("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
				Sfi.Statistics.Clear();
				v.Heigth = 0;
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
				Assert.That(Sfi.Statistics.EntityDeleteCount, Is.EqualTo(0));
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.CreateQuery("delete from EntityWithLifecycle").ExecuteUpdateAsync());
				await (tx.CommitAsync());
			}
		}
	}
}