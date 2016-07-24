#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Insertordering
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InsertOrderingFixtureAsync : TestCaseAsync
	{
		const int batchSize = 10;
		const int instancesPerEach = 12;
		const int typesOfEntities = 3;
		protected override IList Mappings
		{
			get
			{
				return new[]{"Insertordering.Mapping.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect.SupportsSqlBatches;
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.DataBaseIntegration(x =>
				{
					x.BatchSize = batchSize;
					x.OrderInserts = true;
					x.Batcher<StatsBatcherFactory>();
				}

				);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task BatchOrderingAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					for (int i = 0; i < instancesPerEach; i++)
					{
						var user = new User{UserName = "user-" + i};
						var group = new Group{Name = "group-" + i};
						await (s.SaveAsync(user));
						await (s.SaveAsync(group));
						user.AddMembership(group);
					}

					StatsBatcher.Reset();
					await (s.Transaction.CommitAsync());
				}

			int expectedBatchesPerEntity = (instancesPerEach / batchSize) + ((instancesPerEach % batchSize) == 0 ? 0 : 1);
			Assert.That(StatsBatcher.BatchSizes.Count, Is.EqualTo(expectedBatchesPerEntity * typesOfEntities));
			using (ISession s = OpenSession())
			{
				s.BeginTransaction();
				IList users = await (s.CreateQuery("from User u left join fetch u.Memberships m left join fetch m.Group").ListAsync());
				foreach (object user in users)
				{
					await (s.DeleteAsync(user));
				}

				await (s.Transaction.CommitAsync());
			}
		}

#region Nested type: StatsBatcher
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class StatsBatcher : SqlClientBatchingBatcher
		{
			private static string batchSQL;
			private static IList<int> batchSizes = new List<int>();
			private static int currentBatch = -1;
			public StatsBatcher(ConnectionManager connectionManager, IInterceptor interceptor): base (connectionManager, interceptor)
			{
			}

			public static IList<int> BatchSizes
			{
				get
				{
					return batchSizes;
				}
			}

			public static void Reset()
			{
				batchSizes = new List<int>();
				currentBatch = -1;
				batchSQL = null;
			}

			public override DbCommand PrepareBatchCommand(CommandType type, SqlString sql, SqlType[] parameterTypes)
			{
				DbCommand result = base.PrepareBatchCommand(type, sql, parameterTypes);
				string sqlstring = sql.ToString();
				if (batchSQL == null || !sqlstring.Equals(batchSQL))
				{
					currentBatch++;
					batchSQL = sqlstring;
					batchSizes.Insert(currentBatch, 0);
					Console.WriteLine("--------------------------------------------------------");
					Console.WriteLine("Preparing statement [" + batchSQL + "]");
				}

				return result;
			}

			public override void AddToBatch(IExpectation expectation)
			{
				batchSizes[currentBatch]++;
				Console.WriteLine("Adding to batch [" + batchSQL + "]");
				base.AddToBatch(expectation);
			}

			protected override void DoExecuteBatch(DbCommand ps)
			{
				Console.WriteLine("executing batch [" + batchSQL + "]");
				Console.WriteLine("--------------------------------------------------------");
				batchSQL = null;
				base.DoExecuteBatch(ps);
			}
		}

#endregion
#region Nested type: StatsBatcherFactory
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class StatsBatcherFactory : IBatcherFactory
		{
#region IBatcherFactory Members
			public IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
			{
				return new StatsBatcher(connectionManager, interceptor);
			}
#endregion
		}
#endregion
	}
}
#endif
