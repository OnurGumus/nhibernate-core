#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ReadOnly
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadOnlyProxyTest : AbstractReadOnlyTest
	{
		[Test]
		public async Task ReadOnlyViaSessionDoesNotInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.FlushAsync());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.Transaction.CommitAsync());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaLazyInitializerDoesNotInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dpLI.ReadOnly = true;
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dpLI.ReadOnly = false;
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.FlushAsync());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.Transaction.CommitAsync());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaSessionNoChangeAfterInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			CheckReadOnly(s, dp, true);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaLazyInitializerNoChangeAfterInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			CheckReadOnly(s, dp, false);
			Assert.That(dpLI.IsUninitialized);
			NHibernateUtil.Initialize(dp);
			Assert.That(dpLI.IsUninitialized, Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			dpLI.ReadOnly = true;
			CheckReadOnly(s, dp, true);
			Assert.That(dpLI.IsUninitialized);
			NHibernateUtil.Initialize(dp);
			Assert.That(dpLI.IsUninitialized, Is.False);
			CheckReadOnly(s, dp, true);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			dpLI.ReadOnly = true;
			CheckReadOnly(s, dp, true);
			Assert.That(dpLI.IsUninitialized);
			dpLI.ReadOnly = false;
			CheckReadOnly(s, dp, false);
			Assert.That(dpLI.IsUninitialized);
			NHibernateUtil.Initialize(dp);
			Assert.That(dpLI.IsUninitialized, Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaSessionBeforeInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			s.SetReadOnly(dp, true);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ModifiableViaSessionBeforeInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, false);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaSessionBeforeInitByModifiableQueryAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			DataPoint dpFromQuery = s.CreateQuery("from DataPoint where id = " + dpOrig.Id).SetReadOnly(false).UniqueResult<DataPoint>();
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaSessionBeforeInitByReadOnlyQueryAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			DataPoint dpFromQuery = s.CreateQuery("from DataPoint where Id = " + dpOrig.Id).SetReadOnly(true).UniqueResult<DataPoint>();
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ModifiableViaSessionBeforeInitByModifiableQueryAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			DataPoint dpFromQuery = s.CreateQuery("from DataPoint where Id = " + dpOrig.Id).SetReadOnly(false).UniqueResult<DataPoint>();
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ModifiableViaSessionBeforeInitByReadOnlyQueryAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			DataPoint dpFromQuery = s.CreateQuery("from DataPoint where id=" + dpOrig.Id).SetReadOnly(true).UniqueResult<DataPoint>();
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaLazyInitializerBeforeInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			CheckReadOnly(s, dp, false);
			dpLI.ReadOnly = true;
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ModifiableViaLazyInitializerBeforeInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(dpLI.IsUninitialized);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, false);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyViaLazyInitializerAfterInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, false);
			dpLI.ReadOnly = true;
			CheckReadOnly(s, dp, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ModifiableViaLazyInitializerAfterInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, false);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		[Ignore("Failing test. See HHH-4642")]
		public async Task ModifyToReadOnlyToModifiableIsUpdatedFailureExpectedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			try
			{
				Assert.That(dp.Description, Is.EqualTo("changed"));
			// should fail due to HHH-4642
			}
			finally
			{
				s.Transaction.Rollback();
				s.Close();
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		[Ignore("Failing test. See HHH-4642")]
		public async Task ReadOnlyModifiedToModifiableIsUpdatedFailureExpectedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			try
			{
				Assert.That(dp.Description, Is.EqualTo("changed"));
			// should fail due to HHH-4642
			}
			finally
			{
				s.Transaction.Rollback();
				s.Close();
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task ReadOnlyChangedEvictedUpdateAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			s.Evict(dp);
			Assert.That(s.Contains(dp), Is.False);
			await (s.UpdateAsync(dp));
			CheckReadOnly(s, dp, false);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyToModifiableInitWhenModifiedIsUpdatedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyInitToModifiableModifiedIsUpdatedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			CheckReadOnly(s, dp, true);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyModifiedUpdateAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, true);
			await (s.UpdateAsync(dp));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyDeleteAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.DeleteAsync(dp));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.Null);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyRefreshAsync()
		{
			DataPoint dp = await (this.CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			ITransaction t = s.BeginTransaction();
			dp = s.Load<DataPoint>(dp.Id);
			s.SetReadOnly(dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			s.Refresh(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(dp.Description, Is.EqualTo("original"));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(s.IsReadOnly(dp), Is.True);
			Assert.That(s.IsReadOnly(((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementation()), Is.True);
			s.Refresh(dp);
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(s.IsReadOnly(dp), Is.True);
			Assert.That(s.IsReadOnly(((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementation()), Is.True);
			await (t.CommitAsync());
			s.Clear();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dp));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyRefreshDeletedAsync()
		{
			DataPoint dp = await (this.CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			ITransaction t = s.BeginTransaction();
			INHibernateProxy dpProxy = (INHibernateProxy)s.Load<DataPoint>(dp.Id);
			Assert.That(NHibernateUtil.IsInitialized(dpProxy), Is.False);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			await (s.DeleteAsync(dp));
			await (s.FlushAsync());
			try
			{
				s.Refresh(dp);
				Assert.Fail("should have thrown UnresolvableObjectException");
			}
			catch (UnresolvableObjectException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}

			s = OpenSession();
			t = s.BeginTransaction();
			s.CacheMode = CacheMode.Ignore;
			DataPoint dpProxyInit = s.Load<DataPoint>(dp.Id);
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dpProxyInit));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(dpProxyInit, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dpProxyInit), Is.True);
			try
			{
				s.Refresh(dpProxyInit);
				Assert.Fail("should have thrown UnresolvableObjectException");
			}
			catch (UnresolvableObjectException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}

			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(dpProxyInit, Is.InstanceOf<INHibernateProxy>());
			try
			{
				s.Refresh(dpProxy);
				Assert.That(NHibernateUtil.IsInitialized(dpProxy), Is.False);
				NHibernateUtil.Initialize(dpProxy);
				Assert.Fail("should have thrown UnresolvableObjectException");
			}
			catch (UnresolvableObjectException)
			{
			// expected
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}

		[Test]
		public async Task ReadOnlyRefreshDetachedAsync()
		{
			DataPoint dp = await (this.CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			ITransaction t = s.BeginTransaction();
			dp = s.Load<DataPoint>(dp.Id);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(s.IsReadOnly(dp), Is.False);
			s.SetReadOnly(dp, true);
			Assert.That(s.IsReadOnly(dp), Is.True);
			s.Evict(dp);
			s.Refresh(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(s.IsReadOnly(dp), Is.False);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			s.SetReadOnly(dp, true);
			s.Evict(dp);
			s.Refresh(dp);
			Assert.That(dp.Description, Is.EqualTo("original"));
			Assert.That(s.IsReadOnly(dp), Is.False);
			await (t.CommitAsync());
			s.Clear();
			t = s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dp.Id));
			Assert.That(dp.Description, Is.EqualTo("original"));
			await (s.DeleteAsync(dp));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyProxyMergeDetachedProxyWithChangeAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy
			dp.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dpLoaded, false);
			s.SetReadOnly(dpLoaded, true);
			CheckReadOnly(s, dpLoaded, true);
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			DataPoint dpMerged = (DataPoint)s.Merge(dp);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dpLoaded, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyProxyInitMergeDetachedProxyWithChangeAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy
			dp.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			NHibernateUtil.Initialize(dpLoaded);
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			CheckReadOnly(s, dpLoaded, false);
			s.SetReadOnly(dpLoaded, true);
			CheckReadOnly(s, dpLoaded, true);
			DataPoint dpMerged = (DataPoint)s.Merge(dp);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dpLoaded, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyProxyMergeDetachedEntityWithChangeAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy target
			DataPoint dpEntity = (DataPoint)((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementation();
			dpEntity.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dpLoaded, false);
			s.SetReadOnly(dpLoaded, true);
			CheckReadOnly(s, dpLoaded, true);
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			DataPoint dpMerged = (DataPoint)s.Merge(dpEntity);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dpLoaded, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyProxyInitMergeDetachedEntityWithChangeAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy target
			DataPoint dpEntity = (DataPoint)((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementation();
			dpEntity.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			NHibernateUtil.Initialize(dpLoaded);
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			CheckReadOnly(s, dpLoaded, false);
			s.SetReadOnly(dpLoaded, true);
			CheckReadOnly(s, dpLoaded, true);
			DataPoint dpMerged = (DataPoint)s.Merge(dpEntity);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dpLoaded, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ReadOnlyEntityMergeDetachedProxyWithChangeAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			CheckReadOnly(s, dp, false);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			NHibernateUtil.Initialize(dp);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy
			dp.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpEntity = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dpEntity, Is.Not.InstanceOf<INHibernateProxy>());
			Assert.That(s.IsReadOnly(dpEntity), Is.False);
			s.SetReadOnly(dpEntity, true);
			Assert.That(s.IsReadOnly(dpEntity), Is.True);
			DataPoint dpMerged = (DataPoint)s.Merge(dp);
			Assert.That(dpMerged, Is.SameAs(dpEntity));
			Assert.That(dpEntity.Description, Is.EqualTo("changed"));
			Assert.That(s.IsReadOnly(dpEntity), Is.True);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SetReadOnlyInTwoTransactionsSameSessionAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			CheckReadOnly(s, dp, true);
			s.BeginTransaction();
			CheckReadOnly(s, dp, true);
			dp.Description = "changed again";
			Assert.That(dp.Description, Is.EqualTo("changed again"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SetReadOnlyBetweenTwoTransactionsSameSessionAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, false);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			s.BeginTransaction();
			CheckReadOnly(s, dp, true);
			dp.Description = "changed again";
			Assert.That(dp.Description, Is.EqualTo("changed again"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task SetModifiableBetweenTwoTransactionsSameSessionAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.SetReadOnly(dp, true);
			CheckReadOnly(s, dp, true);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			CheckReadOnly(s, dp, true);
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			CheckReadOnly(s, dp, true);
			s.SetReadOnly(dp, false);
			CheckReadOnly(s, dp, false);
			s.BeginTransaction();
			CheckReadOnly(s, dp, false);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			s.Refresh(dp);
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			CheckReadOnly(s, dp, false);
			dp.Description = "changed again";
			Assert.That(dp.Description, Is.EqualTo("changed again"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.BeginTransaction();
			dp = await (s.GetAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp.Id, Is.EqualTo(dpOrig.Id));
			Assert.That(dp.Description, Is.EqualTo("changed again"));
			Assert.That(dp.X, Is.EqualTo(dpOrig.X));
			Assert.That(dp.Y, Is.EqualTo(dpOrig.Y));
			await (s.DeleteAsync(dp));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task IsReadOnlyAfterSessionClosedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			try
			{
				s.IsReadOnly(dp);
				Assert.Fail("should have failed because session was closed");
			}
			catch (ObjectDisposedException) // SessionException in Hibernate
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task IsReadOnlyAfterSessionClosedViaLazyInitializerAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			Assert.That(s.Contains(dp), Is.True);
			s.Close();
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				var value = ((INHibernateProxy)dp).HibernateLazyInitializer.ReadOnly;
				Assert.Fail("should have failed because session was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task DetachedIsReadOnlyAfterEvictViaSessionAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			Assert.That(s.Contains(dp), Is.True);
			s.Evict(dp);
			Assert.That(s.Contains(dp), Is.False);
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				s.IsReadOnly(dp);
				Assert.Fail("should have failed because proxy was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task DetachedIsReadOnlyAfterEvictViaLazyInitializerAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.Evict(dp);
			Assert.That(s.Contains(dp), Is.False);
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				var value = ((INHibernateProxy)dp).HibernateLazyInitializer.ReadOnly;
				Assert.Fail("should have failed because proxy was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task SetReadOnlyAfterSessionClosedAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			s.Close();
			try
			{
				s.SetReadOnly(dp, true);
				Assert.Fail("should have failed because session was closed");
			}
			catch (ObjectDisposedException) // SessionException in Hibernate
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task SetReadOnlyAfterSessionClosedViaLazyInitializerAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			Assert.That(s.Contains(dp), Is.True);
			s.Close();
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				((INHibernateProxy)dp).HibernateLazyInitializer.ReadOnly = true;
				Assert.Fail("should have failed because session was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task SetClosedSessionInLazyInitializerAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			await (s.Transaction.CommitAsync());
			Assert.That(s.Contains(dp), Is.True);
			s.Close();
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			Assert.That(((ISessionImplementor)s).IsClosed, Is.True);
			try
			{
				((INHibernateProxy)dp).HibernateLazyInitializer.SetSession((ISessionImplementor)s);
				Assert.Fail("should have failed because session was closed");
			}
			catch (ObjectDisposedException) // SessionException in Hibernate
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s = OpenSession();
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task DetachedSetReadOnlyAfterEvictViaSessionAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			Assert.That(s.Contains(dp), Is.True);
			s.Evict(dp);
			Assert.That(s.Contains(dp), Is.False);
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				s.SetReadOnly(dp, true);
				Assert.Fail("should have failed because proxy was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task DetachedSetReadOnlyAfterEvictViaLazyInitializerAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = s.Load<DataPoint>(dpOrig.Id);
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			CheckReadOnly(s, dp, false);
			s.Evict(dp);
			Assert.That(s.Contains(dp), Is.False);
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				((INHibernateProxy)dp).HibernateLazyInitializer.ReadOnly = true;
				Assert.Fail("should have failed because proxy was detached");
			}
			catch (TransientObjectException)
			{
				// expected
				Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.IsReadOnlySettingAvailable, Is.False);
			}
			finally
			{
				s.BeginTransaction();
				await (s.DeleteAsync(dp));
				await (s.Transaction.CommitAsync());
				s.Close();
			}
		}

		private async Task<DataPoint> CreateDataPointAsync(CacheMode mode)
		{
			DataPoint dp = null;
			using (ISession s = OpenSession())
			{
				s.CacheMode = CacheMode.Ignore;
				using (ITransaction t = s.BeginTransaction())
				{
					dp = new DataPoint();
					dp.X = 0.1M;
					dp.Y = (decimal)System.Math.Cos((double)dp.X);
					dp.Description = "original";
					await (s.SaveAsync(dp));
					await (t.CommitAsync());
				}
			}

			return dp;
		}
	}
}
#endif
