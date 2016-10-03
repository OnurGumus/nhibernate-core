#if NET_4_5
using System.Collections;
using System.Data;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1677
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityModeMapCriteriaAsync : BugTestCaseAsync
	{
		private const int NumberOfRecordPerEntity = 10;
		private const string Entity1Name = "Entity1";
		private const string Entity1Property = "Entity1Property";
		private const string Entity2Name = "Entity2";
		private const string Entity2Property = "Entity2Property";
		private const string EntityPropertyPrefix = "Record";
		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty("default_entity_mode", EntityModeHelper.ToString(EntityMode.Map));
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction(IsolationLevel.ReadCommitted))
				{
					for (int i = 0; i < NumberOfRecordPerEntity; i++)
					{
						var entity1 = new Hashtable();
						entity1[Entity1Property] = EntityPropertyPrefix + i;
						await (s.SaveOrUpdateAsync(Entity1Name, entity1));
					}

					for (int i = 0; i < NumberOfRecordPerEntity; i++)
					{
						var entity2 = new Hashtable();
						entity2[Entity2Property] = EntityPropertyPrefix + i;
						await (s.SaveOrUpdateAsync(Entity2Name, entity2));
					}

					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(string.Format("from {0}", Entity1Name)));
					await (s.DeleteAsync(string.Format("from {0}", Entity2Name)));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task EntityModeMapFailsWithCriteriaAsync()
		{
			using (ISessionFactory sf = cfg.BuildSessionFactory())
			{
				using (ISession s = sf.OpenSession())
				{
					IQuery query = s.CreateQuery(string.Format("from {0}", Entity1Name));
					IList entity1List = await (query.ListAsync());
					Assert.AreEqual(NumberOfRecordPerEntity, entity1List.Count); // OK, Count == 10
				}

				using (ISession s = sf.OpenSession())
				{
					ICriteria entity1Criteria = s.CreateCriteria(Entity1Name);
					IList entity1List = await (entity1Criteria.ListAsync());
					Assert.AreEqual(NumberOfRecordPerEntity, entity1List.Count); // KO !!! Count == 20 !!!
				}
			}
		}
	}
}
#endif
