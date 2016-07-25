#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3436
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<TestEntity>(rc =>
			{
				rc.Table("TestEntity");
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new TestEntity();
					await (session.SaveAsync(e1));
					var e2 = new TestEntity();
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void TestQueryWithContainsInParallel()
		{
			var ids = new List<Guid>{Guid.NewGuid(), Guid.NewGuid(), };
			const int threadsToRun = 32;
			var events = new WaitHandle[threadsToRun];
			var exceptions = new List<Exception>();
			for (var i = 0; i < threadsToRun; i++)
			{
				var @event = new ManualResetEvent(false);
				events[i] = @event;
				ThreadPool.QueueUserWorkItem(s =>
				{
					try
					{
						Run(ids);
					}
					catch (Exception ex)
					{
						exceptions.Add(ex);
					}
					finally
					{
						@event.Set();
					}
				}

				);
			}

			WaitHandle.WaitAll(events);
			Assert.IsEmpty(exceptions);
		}

		[Test]
		public void TestQueryWithContains()
		{
			var ids = new List<Guid>{Guid.NewGuid(), Guid.NewGuid(), };
			Run(ids);
		}

		private void Run(ICollection<Guid> ids)
		{
			using (var session = sessions.OpenSession())
				using (session.BeginTransaction())
				{
					var result = session.Query<TestEntity>().Where(entity => ids.Contains(entity.Id)).ToList();
					Assert.That(result.Count, Is.EqualTo(0));
				}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class TestEntity
		{
			public virtual Guid Id
			{
				get;
				set;
			}
		}
	}
}
#endif
