#if NET_4_5
using System;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1483
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override Task OnTearDownAsync()
		{
			return DeleteAllAsync(true);
		}

		/// <summary>
		/// Tests that a Subclass can be loaded from second level cache as the specified 
		/// type of baseclass
		/// </summary>
		/// <typeparam name = "TBaseClass">The type of the BaseClass to test.</typeparam>
		public async Task TestLoadFromSecondLevelCacheAsync<TBaseClass>()where TBaseClass : BaseClass
		{
			//create a new persistent entity to work with
			Guid id = (await (CreateAndSaveNewSubclassAsync())).Id;
			using (ISession session = OpenSession())
			{
				//make sure the entity can be pulled
				TBaseClass entity = await (session.GetAsync<TBaseClass>(id));
				Assert.IsNotNull(entity);
			}

			await (DeleteAllAsync(false));
			using (ISession session = OpenSession())
			{
				//reload the subclass, this should pull it directly from cache
				TBaseClass restoredEntity = await (session.GetAsync<TBaseClass>(id));
				Assert.IsNotNull(restoredEntity);
			}
		}

		/// <summary>
		/// Creates and save a new subclass to the database.
		/// </summary>
		/// <returns>the new persistent SubClass</returns>
		private async Task<SubClass> CreateAndSaveNewSubclassAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					SubClass entity = new SubClass();
					await (session.SaveAsync(entity));
					await (trans.CommitAsync());
					return entity;
				}
			}
		}

		/// <summary>
		/// Deletes all the baseclass entities from the persistence medium
		/// </summary>
		/// <param name = "inNHibernateScope">whether to delete the entities though NHibernate
		/// scope our outside of the scope so that entities will still remain in the session cache</param>
		private async Task DeleteAllAsync(bool inNHibernateScope)
		{
			using (ISession session = OpenSession())
			{
				if (inNHibernateScope)
				{
					using (ITransaction trans = session.BeginTransaction())
					{
						await (session.DeleteAsync("from BaseClass"));
						await (trans.CommitAsync());
					}
				}
				else
				{
					//delete directly from the db
					using (DbCommand cmd = session.Connection.CreateCommand())
					{
						cmd.CommandText = "DELETE FROM BaseClass";
						await (cmd.ExecuteNonQueryAsync());
					}
				}
			}
		}

		/// <summary>
		/// Verifies that a subclass can be loaded from the second level cache
		/// </summary>
		[Test]
		public async Task LoadSubclassFromSecondLevelCacheAsync()
		{
			await (TestLoadFromSecondLevelCacheAsync<SubClass>());
		}

		/// <summary>
		/// Verifies that a subclass can be loaded from the second level cache
		/// </summary>
		[Test]
		public async Task LoadSubclassFromSecondLevelCacheAsBaseClassAsync()
		{
			await (TestLoadFromSecondLevelCacheAsync<BaseClass>());
		}
	}
}
#endif
