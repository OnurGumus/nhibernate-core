﻿#if NET_4_5
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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadOnlyProxyTestAsync : AbstractReadOnlyTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"ReadOnly.DataPoint.hbm.xml", //"ReadOnly.TextHolder.hbm.xml"
				};
			}
		}

		[Test]
		public async Task ReadOnlyViaSessionDoesNotInitAsync()
		{
			DataPoint dpOrig = await (CreateDataPointAsync(CacheMode.Ignore));
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.FlushAsync());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.Transaction.CommitAsync());
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dpLI.ReadOnly = true;
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dpLI.ReadOnly = false;
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.FlushAsync());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.Transaction.CommitAsync());
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(dpLI.IsUninitialized);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(dpLI.IsUninitialized, Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			dpLI.ReadOnly = true;
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(dpLI.IsUninitialized);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(dpLI.IsUninitialized, Is.False);
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			dpLI.ReadOnly = true;
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(dpLI.IsUninitialized);
			dpLI.ReadOnly = false;
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(dpLI.IsUninitialized);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(dpLI.IsUninitialized, Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (s.SetReadOnlyAsync(dp, true));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			DataPoint dpFromQuery = await (s.CreateQuery("from DataPoint where id = " + dpOrig.Id).SetReadOnly(false).UniqueResultAsync<DataPoint>());
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			DataPoint dpFromQuery = await (s.CreateQuery("from DataPoint where Id = " + dpOrig.Id).SetReadOnly(true).UniqueResultAsync<DataPoint>());
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			DataPoint dpFromQuery = await (s.CreateQuery("from DataPoint where Id = " + dpOrig.Id).SetReadOnly(false).UniqueResultAsync<DataPoint>());
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			DataPoint dpFromQuery = await (s.CreateQuery("from DataPoint where id=" + dpOrig.Id).SetReadOnly(true).UniqueResultAsync<DataPoint>());
			Assert.That(NHibernateUtil.IsInitialized(dpFromQuery), Is.True);
			Assert.That(dpFromQuery, Is.SameAs(dp));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			await (CheckReadOnlyAsync(s, dp, false));
			dpLI.ReadOnly = true;
			await (CheckReadOnlyAsync(s, dp, true));
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(dpLI.IsUninitialized);
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, false));
			dpLI.ReadOnly = true;
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer dpLI = ((INHibernateProxy)dp).HibernateLazyInitializer;
			Assert.That(dpLI.IsUninitialized);
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(dpLI.IsUninitialized, Is.False);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.EvictAsync(dp));
			Assert.That(await (s.ContainsAsync(dp)), Is.False);
			await (s.UpdateAsync(dp));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
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
			dp = await (s.LoadAsync<DataPoint>(dp.Id));
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (s.RefreshAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(dp.Description, Is.EqualTo("original"));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(s.IsReadOnly(dp), Is.True);
			Assert.That(s.IsReadOnly(await (((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementationAsync())), Is.True);
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo("original"));
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(s.IsReadOnly(dp), Is.True);
			Assert.That(s.IsReadOnly(await (((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementationAsync())), Is.True);
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
			INHibernateProxy dpProxy = (INHibernateProxy)await (s.LoadAsync<DataPoint>(dp.Id));
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
				await (s.RefreshAsync(dp));
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
			DataPoint dpProxyInit = await (s.LoadAsync<DataPoint>(dp.Id));
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
				await (s.RefreshAsync(dpProxyInit));
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
				await (s.RefreshAsync(dpProxy));
				Assert.That(NHibernateUtil.IsInitialized(dpProxy), Is.False);
				await (NHibernateUtil.InitializeAsync(dpProxy));
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
			dp = await (s.LoadAsync<DataPoint>(dp.Id));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(s.IsReadOnly(dp), Is.False);
			await (s.SetReadOnlyAsync(dp, true));
			Assert.That(s.IsReadOnly(dp), Is.True);
			await (s.EvictAsync(dp));
			await (s.RefreshAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			Assert.That(s.IsReadOnly(dp), Is.False);
			dp.Description = "changed";
			Assert.That(dp.Description, Is.EqualTo("changed"));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.SetReadOnlyAsync(dp, true));
			await (s.EvictAsync(dp));
			await (s.RefreshAsync(dp));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy
			dp.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dpLoaded, false));
			await (s.SetReadOnlyAsync(dpLoaded, true));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			DataPoint dpMerged = (DataPoint)s.Merge(dp);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy
			dp.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			await (NHibernateUtil.InitializeAsync(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			await (CheckReadOnlyAsync(s, dpLoaded, false));
			await (s.SetReadOnlyAsync(dpLoaded, true));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
			DataPoint dpMerged = (DataPoint)s.Merge(dp);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy target
			DataPoint dpEntity = (DataPoint)await (((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementationAsync());
			dpEntity.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dpLoaded, false));
			await (s.SetReadOnlyAsync(dpLoaded, true));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			DataPoint dpMerged = (DataPoint)s.Merge(dpEntity);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			await (s.Transaction.CommitAsync());
			s.Close();
			// modify detached proxy target
			DataPoint dpEntity = (DataPoint)await (((INHibernateProxy)dp).HibernateLazyInitializer.GetImplementationAsync());
			dpEntity.Description = "changed";
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			DataPoint dpLoaded = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dpLoaded, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.False);
			await (NHibernateUtil.InitializeAsync(dpLoaded));
			Assert.That(NHibernateUtil.IsInitialized(dpLoaded), Is.True);
			await (CheckReadOnlyAsync(s, dpLoaded, false));
			await (s.SetReadOnlyAsync(dpLoaded, true));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
			DataPoint dpMerged = (DataPoint)s.Merge(dpEntity);
			Assert.That(dpMerged, Is.SameAs(dpLoaded));
			Assert.That(dpLoaded.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dpLoaded, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (NHibernateUtil.InitializeAsync(dp));
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
			await (s.SetReadOnlyAsync(dpEntity, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			await (CheckReadOnlyAsync(s, dp, true));
			s.BeginTransaction();
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			s.BeginTransaction();
			await (CheckReadOnlyAsync(s, dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.SetReadOnlyAsync(dp, true));
			await (CheckReadOnlyAsync(s, dp, true));
			dp.Description = "changed";
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.True);
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.FlushAsync());
			await (s.Transaction.CommitAsync());
			await (CheckReadOnlyAsync(s, dp, true));
			await (s.SetReadOnlyAsync(dp, false));
			await (CheckReadOnlyAsync(s, dp, false));
			s.BeginTransaction();
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(dp.Description, Is.EqualTo("changed"));
			await (s.RefreshAsync(dp));
			Assert.That(dp.Description, Is.EqualTo(dpOrig.Description));
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			Assert.That(await (s.ContainsAsync(dp)), Is.True);
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(await (s.ContainsAsync(dp)), Is.True);
			await (s.EvictAsync(dp));
			Assert.That(await (s.ContainsAsync(dp)), Is.False);
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.EvictAsync(dp));
			Assert.That(await (s.ContainsAsync(dp)), Is.False);
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			try
			{
				await (s.SetReadOnlyAsync(dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			Assert.That(await (s.ContainsAsync(dp)), Is.True);
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.Transaction.CommitAsync());
			Assert.That(await (s.ContainsAsync(dp)), Is.True);
			s.Close();
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			Assert.That(((ISessionImplementor)s).IsClosed, Is.True);
			try
			{
				await (((INHibernateProxy)dp).HibernateLazyInitializer.SetSessionAsync((ISessionImplementor)s));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			Assert.That(await (s.ContainsAsync(dp)), Is.True);
			await (s.EvictAsync(dp));
			Assert.That(await (s.ContainsAsync(dp)), Is.False);
			Assert.That(((INHibernateProxy)dp).HibernateLazyInitializer.Session, Is.Null);
			try
			{
				await (s.SetReadOnlyAsync(dp, true));
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
			DataPoint dp = await (s.LoadAsync<DataPoint>(dpOrig.Id));
			Assert.That(dp, Is.InstanceOf<INHibernateProxy>());
			Assert.That(NHibernateUtil.IsInitialized(dp), Is.False);
			await (CheckReadOnlyAsync(s, dp, false));
			await (s.EvictAsync(dp));
			Assert.That(await (s.ContainsAsync(dp)), Is.False);
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

		private async Task CheckReadOnlyAsync(ISession s, object proxy, bool expectedReadOnly)
		{
			Assert.That(proxy, Is.InstanceOf<INHibernateProxy>());
			ILazyInitializer li = ((INHibernateProxy)proxy).HibernateLazyInitializer;
			Assert.That(s, Is.SameAs(li.Session));
			Assert.That(s.IsReadOnly(proxy), Is.EqualTo(expectedReadOnly));
			Assert.That(li.ReadOnly, Is.EqualTo(expectedReadOnly));
			Assert.That(NHibernateUtil.IsInitialized(proxy), Is.Not.EqualTo(li.IsUninitialized));
			if (NHibernateUtil.IsInitialized(proxy))
			{
				Assert.That(s.IsReadOnly(await (li.GetImplementationAsync())), Is.EqualTo(expectedReadOnly));
			}
		}
	}
}
#endif
