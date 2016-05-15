#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ReadOnly
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadOnlyTest : AbstractReadOnlyTest
	{
		[Test]
		public async Task ReadOnlyOnProxiesAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			dp.Description = "original";
			await (s.SaveAsync(dp));
			long dpId = dp.Id;
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.LoadAsync<DataPoint>(dpId));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False, "was initialized");
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False, "was initialized during SetReadOnly");
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True, "was not initialized during mod");
			Assert.That(dp.Description, Is.EqualTo("changed"), "desc not changed in memory");
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			IList list = await (s.CreateQuery("from DataPoint where Description = 'changed'").ListAsync());
			Assert.That(list.Count, Is.EqualTo(0), "change written to database");
			Assert.That(await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync()), Is.EqualTo(1));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
		//deletes from Query.executeUpdate() are not tracked
		//AssertDeleteCount(1);
		}

		public async Task ReadOnlyModeAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			for (int i = 0; i < 100; i++)
			{
				DataPoint dp = new DataPoint();
				dp.X = i * 0.1M;
				dp.Y = (decimal)System.Math.Cos((double)dp.X);
				await (s.SaveAsync(dp));
			}

			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(100);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			// NH-specific: Replace use of Scroll with List
			IList<DataPoint> sr = await (s.CreateQuery("from DataPoint dp order by dp.X asc").SetReadOnly(true).ListAsync<DataPoint>());
			int index = 0;
			foreach (DataPoint dp in sr)
			{
				if (++index == 50)
				{
					await (s.SetReadOnlyAsync(dp, false));
				}

				dp.Description = "done!";
			}

			await (t.CommitAsync());
			AssertUpdateCount(1);
			ClearCounts();
			s.Clear();
			t = s.BeginTransaction();
			IList single = await (s.CreateQuery("from DataPoint where description='done!'").ListAsync());
			Assert.That(single.Count, Is.EqualTo(1));
			Assert.That(await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync()), Is.EqualTo(100));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
		//deletes from Query.executeUpdate() are not tracked
		//AssertDeleteCount(100);
		}

		[Test]
		public async Task ReadOnlyModeAutoFlushOnQueryAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			for (int i = 0; i < 100; i++)
			{
				DataPoint dp = new DataPoint();
				dp.X = i * 0.1M;
				dp.Y = (decimal)System.Math.Cos((double)dp.X);
				await (s.SaveAsync(dp));
			}

			AssertInsertCount(0);
			AssertUpdateCount(0);
			// NH-specific: Replace use of Scroll with List
			IList<DataPoint> sr = await (s.CreateQuery("from DataPoint dp order by dp.X asc").SetReadOnly(true).ListAsync<DataPoint>());
			AssertInsertCount(100);
			AssertUpdateCount(0);
			ClearCounts();
			foreach (DataPoint dp in sr)
			{
				Assert.That(s.IsReadOnly(dp), Is.False);
				await (s.DeleteAsync(dp));
			}

			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(100);
		}

		[Test]
		public async Task SaveReadOnlyModifyInSaveTransactionAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.Description = "original";
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (s.SetReadOnlyAsync(dp, true));
			dp.Description = "different";
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (t.CommitAsync());
			AssertInsertCount(0);
			AssertUpdateCount(0);
			s.Clear();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
			ClearCounts();
		}

		[Test]
		public async Task ReadOnlyRefreshAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.Description = "original";
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (t.CommitAsync());
			AssertInsertCount(0);
			AssertUpdateCount(0);
			s.Clear();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dp));
			;
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
			ClearCounts();
		}

		[Test]
		public async Task ReadOnlyRefreshDetachedAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.Description = "original";
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo("original"));
			Assert.That(s.IsReadOnly(dp), Is.False);
			await (s.SetReadOnlyAsync(dp, true));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.EvictAsync(dp));
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo("original"));
			Assert.That(s.IsReadOnly(dp), Is.False);
			await (t.CommitAsync());
			AssertInsertCount(0);
			AssertUpdateCount(0);
			s.Clear();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dp));
			;
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public async Task ReadOnlyDeleteAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dp, true));
			await (s.DeleteAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
			s = OpenSession();
			t = s.BeginTransaction();
			IList list = await (s.CreateQuery("from DataPoint where Description='done!'").ListAsync());
			Assert.That(list.Count, Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyGetModifyAndDeleteAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dp, true));
			dp.Description = "a DataPoint";
			await (s.DeleteAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			IList list = await (s.CreateQuery("from DataPoint where Description='done!'").ListAsync());
			Assert.That(list.Count, Is.EqualTo(0));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyModeWithExistingModifiableEntityAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = null;
			for (int i = 0; i < 100; i++)
			{
				dp = new DataPoint();
				dp.X = i * 0.1M;
				dp.Y = (decimal)System.Math.Cos((double)dp.X);
				await (s.SaveAsync(dp));
			}

			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(100);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			DataPoint dpLast = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(s.IsReadOnly(dpLast), Is.False);
			// NH-specific: Replace use of Scroll with List
			IList<DataPoint> sr = await (s.CreateQuery("from DataPoint dp order by dp.X asc").SetReadOnly(true).ListAsync<DataPoint>());
			int nExpectedChanges = 0;
			int index = 0;
			foreach (DataPoint nextDp in sr)
			{
				if (nextDp.Id == dpLast.Id)
				{
					//dpLast existed in the session before executing the read-only query
					Assert.That(s.IsReadOnly(nextDp), Is.False);
				}
				else
				{
					Assert.That(s.IsReadOnly(nextDp), Is.True);
				}

				if (++index == 50)
				{
					await (s.SetReadOnlyAsync(nextDp, false));
					nExpectedChanges = (nextDp == dpLast ? 1 : 2);
				}

				nextDp.Description = "done!";
			}

			await (t.CommitAsync());
			s.Clear();
			AssertInsertCount(0);
			AssertUpdateCount(nExpectedChanges);
			ClearCounts();
			t = s.BeginTransaction();
			IList list = await (s.CreateQuery("from DataPoint where Description='done!'").ListAsync());
			Assert.That(list.Count, Is.EqualTo(nExpectedChanges));
			Assert.That(await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync()), Is.EqualTo(100));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
		}

		[Test]
		public async Task ModifiableModeWithExistingReadOnlyEntityAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = null;
			for (int i = 0; i < 100; i++)
			{
				dp = new DataPoint();
				dp.X = i * 0.1M;
				dp.Y = (decimal)System.Math.Cos((double)dp.X);
				await (s.SaveAsync(dp));
			}

			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(100);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			t = s.BeginTransaction();
			DataPoint dpLast = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(s.IsReadOnly(dpLast), Is.False);
			await (s.SetReadOnlyAsync(dpLast, true));
			Assert.That(s.IsReadOnly(dpLast), Is.True);
			dpLast.Description = "oy";
			AssertUpdateCount(0);
			// NH-specific: Replace use of Scroll with List
			IList<DataPoint> sr = await (s.CreateQuery("from DataPoint dp order by dp.X asc").SetReadOnly(false).ListAsync<DataPoint>());
			int nExpectedChanges = 0;
			int index = 0;
			foreach (DataPoint nextDp in sr)
			{
				if (nextDp.Id == dpLast.Id)
				{
					//dpLast existed in the session before executing the read-only query
					Assert.That(s.IsReadOnly(nextDp), Is.True);
				}
				else
				{
					Assert.That(s.IsReadOnly(nextDp), Is.False);
				}

				if (++index == 50)
				{
					await (s.SetReadOnlyAsync(nextDp, true));
					nExpectedChanges = (nextDp == dpLast ? 99 : 98);
				}

				nextDp.Description = "done!";
			}

			await (t.CommitAsync());
			s.Clear();
			AssertUpdateCount(nExpectedChanges);
			ClearCounts();
			t = s.BeginTransaction();
			IList list = await (s.CreateQuery("from DataPoint where Description='done!'").ListAsync());
			Assert.That(list.Count, Is.EqualTo(nExpectedChanges));
			Assert.That(await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync()), Is.EqualTo(100));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
		}

		[Test]
		public async Task ReadOnlyOnTextTypeAsync()
		{
			if (!TextHolder.SupportedForDialect(Dialect))
				Assert.Ignore("Dialect doesn't support the 'text' data type.");
			string origText = "some huge text string";
			string newText = "some even bigger text string";
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			TextHolder holder = new TextHolder(origText);
			await (s.SaveAsync(holder));
			long id = holder.Id;
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			holder = await (s.GetAsync<TextHolder>(id));
			await (s.SetReadOnlyAsync(holder, true));
			holder.TheText = newText;
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			holder = await (s.GetAsync<TextHolder>(id));
			Assert.That(origText, Is.EqualTo(holder.TheText), "change written to database");
			await (s.DeleteAsync(holder));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public async Task MergeWithReadOnlyEntityAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			DataPoint dp = new DataPoint();
			dp.X = 0.1M;
			dp.Y = (decimal)System.Math.Cos((double)dp.X);
			await (s.SaveAsync(dp));
			await (t.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(0);
			ClearCounts();
			dp.Description = "description";
			s = OpenSession();
			t = s.BeginTransaction();
			DataPoint dpManaged = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dpManaged, true));
			DataPoint dpMerged = (DataPoint)await (s.MergeAsync(dp));
			Assert.That(dpManaged, Is.SameAs(dpMerged));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			s = OpenSession();
			t = s.BeginTransaction();
			dpManaged = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dpManaged.Description, Is.Null);
			await (s.DeleteAsync(dpManaged));
			await (t.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}
	}
}
#endif
