#if NET_4_5
using System;
using System.Diagnostics;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3141
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProxyIdFixtureAsync : BugTestCaseAsync
	{
		private int id;
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					id = (int)await (s.SaveAsync(new Entity()));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Entity e").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}

		[Test, Explicit("No logical test - just to compare before/after fix")]
		public async Task ShouldUseIdDirectlyFromProxyAsync()
		{
			var proxyEntity = await (CreateInitializedProxyAsync());
			const int loop = 1000000;
			var watch = new Stopwatch();
			watch.Start();
			const int dummyValue = 0;
			for (var i = 0; i < loop; i++)
			{
				dummyValue.CompareTo(proxyEntity.Id);
			}

			watch.Stop();
			//before fix: 2.2s
			//after fix: 0.8s
			Console.WriteLine(watch.Elapsed);
		}

		[Test]
		public async Task ShouldThrowExceptionIfIdChangedOnUnloadEntityAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var entity = await (s.LoadAsync<Entity>(id));
					entity.Id++;
					Assert.ThrowsAsync<HibernateException>(async () => await tx.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldThrowExceptionIfIdChangedOnLoadEntityAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var entity = await (s.LoadAsync<Entity>(id));
					await (NHibernateUtil.InitializeAsync(entity));
					entity.Id++;
					Assert.ThrowsAsync<HibernateException>(async () => await tx.CommitAsync());
				}
		}

		private async Task<Entity> CreateInitializedProxyAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var proxyEntity = await (s.LoadAsync<Entity>(id));
					await (NHibernateUtil.InitializeAsync(proxyEntity));
					return proxyEntity;
				}
		}
	}
}
#endif
