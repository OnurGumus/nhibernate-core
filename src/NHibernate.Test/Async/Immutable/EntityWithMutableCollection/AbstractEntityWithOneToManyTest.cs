﻿#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Impl;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Immutable.EntityWithMutableCollection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityWithOneToManyTestAsync : TestCaseAsync
	{
		private bool isContractPartiesInverse;
		private bool isContractPartiesBidirectional;
		private bool isContractVariationsBidirectional;
		private bool isContractVersioned;
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			configuration.SetProperty(NHibernate.Cfg.Environment.GenerateStatistics, "true");
			configuration.SetProperty(NHibernate.Cfg.Environment.BatchSize, "0");
		}

		protected virtual bool CheckUpdateCountsAfterAddingExistingElement()
		{
			return true;
		}

		protected virtual bool CheckUpdateCountsAfterRemovingElementWithoutDelete()
		{
			return true;
		}

		protected override Task OnSetUpAsync()
		{
			try
			{
				isContractPartiesInverse = sessions.GetCollectionPersister(typeof (Contract).FullName + ".Parties").IsInverse;
				try
				{
					sessions.GetEntityPersister(typeof (Party).FullName).GetPropertyType("Contract");
					isContractPartiesBidirectional = true;
				}
				catch (QueryException x)
				{
					isContractPartiesBidirectional = false;
				}

				try
				{
					sessions.GetEntityPersister(typeof (ContractVariation).FullName).GetPropertyType("Contract");
					isContractVariationsBidirectional = true;
				}
				catch (QueryException x)
				{
					isContractVariationsBidirectional = false;
				}

				isContractVersioned = sessions.GetEntityPersister(typeof (Contract).FullName).IsVersioned;
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public virtual async Task UpdatePropertyAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			c.AddParty(new Party("party"));
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			c.CustomerName = "yogi";
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			Party party = c.Parties.First();
			party.Name = "new party";
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party"));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.SameAs(c));
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task CreateWithNonEmptyOneToManyCollectionOfNewAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			c.AddParty(new Party("party"));
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			Party party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party"));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.SameAs(c));
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task CreateWithNonEmptyOneToManyCollectionOfExistingAsync()
		{
			ClearCounts();
			Party party = new Party("party");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			c.AddParty(party);
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.SaveAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			// BUG, should be assertUpdateCount( ! isContractPartiesInverse && isPartyVersioned ? 1 : 0 );
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				party = await (s.CreateCriteria<Party>().UniqueResultAsync<Party>());
				Assert.That(party.Contract, Is.Null);
				await (s.DeleteAsync(party));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task AddNewOneToManyElementToPersistentEntityAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.GetAsync<Contract>(c.Id));
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			c.AddParty(new Party("party"));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			Party party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party"));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.SameAs(c));
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task AddExistingOneToManyElementToPersistentEntityAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (s.PersistAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.GetAsync<Contract>(c.Id));
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			party = await (s.GetAsync<Party>(party.Id));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.Null);
			}

			c.AddParty(party);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			if (CheckUpdateCountsAfterAddingExistingElement())
			{
				AssertUpdateCount(isContractVersioned && !isContractPartiesInverse ? 1 : 0);
			}

			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				await (s.DeleteAsync(party));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task CreateWithEmptyOneToManyCollectionUpdateWithExistingElementAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (s.PersistAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.AddParty(party);
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			if (CheckUpdateCountsAfterAddingExistingElement())
			{
				AssertUpdateCount(isContractVersioned && !isContractPartiesInverse ? 1 : 0);
			}

			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				await (s.DeleteAsync(party));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task CreateWithNonEmptyOneToManyCollectionUpdateWithNewElementAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			Party newParty = new Party("new party");
			c.AddParty(newParty);
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(2));
			foreach (Party aParty in c.Parties)
			{
				if (aParty.Id == party.Id)
				{
					Assert.That(aParty.Name, Is.EqualTo("party"));
				}
				else if (aParty.Id == newParty.Id)
				{
					Assert.That(aParty.Name, Is.EqualTo("new party"));
				}
				else
				{
					Assert.Fail("unknown party");
				}

				if (isContractPartiesBidirectional)
				{
					Assert.That(aParty.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public virtual async Task CreateWithEmptyOneToManyCollectionMergeWithExistingElementAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (s.PersistAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.AddParty(party);
			s = OpenSession();
			t = s.BeginTransaction();
			c = (Contract)s.Merge(c);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			if (CheckUpdateCountsAfterAddingExistingElement())
			{
				AssertUpdateCount(isContractVersioned && !isContractPartiesInverse ? 1 : 0);
			}

			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				await (s.DeleteAsync(party));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task CreateWithNonEmptyOneToManyCollectionMergeWithNewElementAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			Party newParty = new Party("new party");
			c.AddParty(newParty);
			s = OpenSession();
			t = s.BeginTransaction();
			c = (Contract)s.Merge(c);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(2));
			foreach (Party aParty in c.Parties)
			{
				if (aParty.Id == party.Id)
				{
					Assert.That(aParty.Name, Is.EqualTo("party"));
				}
				else if (!aParty.Name.Equals(newParty.Name))
				{
					Assert.Fail("unknown party:" + aParty.Name);
				}

				if (isContractPartiesBidirectional)
				{
					Assert.That(aParty.Contract, Is.SameAs(c));
				}
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public virtual async Task MoveOneToManyElementToNewEntityCollectionAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			c.AddParty(new Party("party"));
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			Party party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party"));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.SameAs(c));
			}

			c.RemoveParty(party);
			Contract c2 = new Contract(null, "david", "phone");
			c2.AddParty(party);
			await (s.SaveAsync(c2));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c.Id)).UniqueResultAsync<Contract>());
			c2 = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c2.Id)).UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}

				Assert.That(c2.Parties.Count, Is.EqualTo(0));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				Assert.That(c2.Parties.Count, Is.EqualTo(1));
				party = c2.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c2));
				}
			}

			await (s.DeleteAsync(c));
			await (s.DeleteAsync(c2));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public virtual async Task MoveOneToManyElementToExistingEntityCollectionAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			c.AddParty(new Party("party"));
			Contract c2 = new Contract(null, "david", "phone");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (s.PersistAsync(c2));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c.Id)).UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(1));
			Party party = c.Parties.First();
			Assert.That(party.Name, Is.EqualTo("party"));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.SameAs(c));
			}

			c.RemoveParty(party);
			c2 = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c2.Id)).UniqueResultAsync<Contract>());
			c2.AddParty(party);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(0);
			AssertUpdateCount(isContractVersioned ? 2 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c.Id)).UniqueResultAsync<Contract>());
			c2 = await (s.CreateCriteria<Contract>().Add(Restrictions.IdEq(c2.Id)).UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c));
				}

				Assert.That(c2.Parties.Count, Is.EqualTo(0));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				Assert.That(c2.Parties.Count, Is.EqualTo(1));
				party = c2.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.SameAs(c2));
				}
			}

			await (s.DeleteAsync(c));
			await (s.DeleteAsync(c2));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public virtual async Task RemoveOneToManyElementUsingUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.RemoveParty(party);
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.Null);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (s.UpdateAsync(party));
			await (t.CommitAsync());
			s.Close();
			if (CheckUpdateCountsAfterRemovingElementWithoutDelete())
			{
				AssertUpdateCount(isContractVersioned && !isContractPartiesInverse ? 1 : 0);
			}

			AssertDeleteCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				Assert.That(party.Contract, Is.SameAs(c));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				party = await (s.CreateCriteria<Party>().UniqueResultAsync<Party>());
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.Null);
				}

				await (s.DeleteAsync(party));
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task RemoveOneToManyElementUsingMergeAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.RemoveParty(party);
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.Null);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			c = (Contract)s.Merge(c);
			party = (Party)s.Merge(party);
			await (t.CommitAsync());
			s.Close();
			if (CheckUpdateCountsAfterRemovingElementWithoutDelete())
			{
				AssertUpdateCount(isContractVersioned && !isContractPartiesInverse ? 1 : 0);
			}

			AssertDeleteCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			if (isContractPartiesInverse)
			{
				Assert.That(c.Parties.Count, Is.EqualTo(1));
				party = c.Parties.First();
				Assert.That(party.Name, Is.EqualTo("party"));
				Assert.That(party.Contract, Is.SameAs(c));
			}
			else
			{
				Assert.That(c.Parties.Count, Is.EqualTo(0));
				party = await (s.CreateCriteria<Party>().UniqueResultAsync<Party>());
				if (isContractPartiesBidirectional)
				{
					Assert.That(party.Contract, Is.Null);
				}

				await (s.DeleteAsync(party));
			}

			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public virtual async Task DeleteOneToManyElementAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			c.RemoveParty(party);
			await (s.DeleteAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			party = await (s.CreateCriteria<Party>().UniqueResultAsync<Party>());
			Assert.That(party, Is.Null);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public virtual async Task RemoveOneToManyElementByDeleteAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			Party party = new Party("party");
			c.AddParty(party);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.RemoveParty(party);
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			if (isContractPartiesBidirectional)
			{
				Assert.That(party.Contract, Is.Null);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (s.DeleteAsync(party));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Parties.Count, Is.EqualTo(0));
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public virtual async Task RemoveOneToManyOrphanUsingUpdateAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			ContractVariation cv = new ContractVariation(c);
			cv.Text = "cv1";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.Variations.Remove(cv);
			cv.Contract = null;
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			if (isContractVariationsBidirectional)
			{
				Assert.That(cv.Contract, Is.Null);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			cv = await (s.CreateCriteria<ContractVariation>().UniqueResultAsync<ContractVariation>());
			Assert.That(cv, Is.Null);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public virtual async Task RemoveOneToManyOrphanUsingMergeAsync()
		{
			Contract c = new Contract(null, "gail", "phone");
			ContractVariation cv = new ContractVariation(c);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			c.Variations.Remove(cv);
			cv.Contract = null;
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			if (isContractVariationsBidirectional)
			{
				Assert.That(cv.Contract, Is.Null);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			c = (Contract)s.Merge(c);
			cv = (ContractVariation)s.Merge(cv);
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			cv = await (s.CreateCriteria<ContractVariation>().UniqueResultAsync<ContractVariation>());
			Assert.That(cv, Is.Null);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public virtual async Task DeleteOneToManyOrphanAsync()
		{
			ClearCounts();
			Contract c = new Contract(null, "gail", "phone");
			ContractVariation cv = new ContractVariation(c);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(c));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.UpdateAsync(c));
			c.Variations.Remove(cv);
			cv.Contract = null;
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			await (s.DeleteAsync(cv));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			Assert.That(c.Variations.Count, Is.EqualTo(0));
			cv = await (s.CreateCriteria<ContractVariation>().UniqueResultAsync<ContractVariation>());
			Assert.That(cv, Is.Null);
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<ContractVariation>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public virtual async Task OneToManyCollectionOptimisticLockingWithMergeAsync()
		{
			ClearCounts();
			Contract cOrig = new Contract(null, "gail", "phone");
			Party partyOrig = new Party("party");
			cOrig.AddParty(partyOrig);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(cOrig));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			Contract c = await (s.GetAsync<Contract>(cOrig.Id));
			Party newParty = new Party("new party");
			c.AddParty(newParty);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cOrig.RemoveParty(partyOrig);
			try
			{
				s.Merge(cOrig);
				Assert.That(isContractVersioned, Is.False);
			}
			catch (StaleObjectStateException)
			{
				Assert.That(isContractVersioned, Is.True);
			}
			finally
			{
				t.Rollback();
			}

			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(3);
		}

		[Test]
		public virtual async Task OneToManyCollectionOptimisticLockingWithUpdateAsync()
		{
			ClearCounts();
			Contract cOrig = new Contract(null, "gail", "phone");
			Party partyOrig = new Party("party");
			cOrig.AddParty(partyOrig);
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.PersistAsync(cOrig));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			Contract c = await (s.GetAsync<Contract>(cOrig.Id));
			Party newParty = new Party("new party");
			c.AddParty(newParty);
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(isContractVersioned ? 1 : 0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			cOrig.RemoveParty(partyOrig);
			await (s.UpdateAsync(cOrig));
			try
			{
				await (t.CommitAsync());
				Assert.That(isContractVersioned, Is.False);
			}
			catch (StaleObjectStateException)
			{
				Assert.That(isContractVersioned, Is.True);
				t.Rollback();
			}

			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			c = await (s.CreateCriteria<Contract>().UniqueResultAsync<Contract>());
			await (s.CreateQuery("delete from Party").ExecuteUpdateAsync());
			await (s.DeleteAsync(c));
			Assert.That(await (s.CreateCriteria<Contract>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			Assert.That(await (s.CreateCriteria<Party>().SetProjection(Projections.RowCountInt64()).UniqueResultAsync<long>()), Is.EqualTo(0L));
			await (t.CommitAsync());
			s.Close();
		}

		protected void ClearCounts()
		{
			Sfi.Statistics.Clear();
		}

		protected void AssertUpdateCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(count), "unexpected update counts");
		}

		protected void AssertInsertCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(count), "unexpected insert count");
		}

		protected void AssertDeleteCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityDeleteCount, Is.EqualTo(count), "unexpected delete counts");
		}
	}
}
#endif
