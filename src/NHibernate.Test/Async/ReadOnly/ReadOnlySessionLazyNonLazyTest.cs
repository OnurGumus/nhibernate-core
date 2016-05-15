#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
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
	public partial class ReadOnlySessionLazyNonLazyTest : AbstractReadOnlyTest
	{
		[Test]
		public async Task ExistingModifiableAfterSetSessionReadOnlyAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Container c = await (s.LoadAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.SameAs(c));
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.SameAs(c));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (s.RefreshAsync(cOrig));
			Assert.That(cOrig, Is.SameAs(c));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			// NH-specific: The following line is required to evict DataPoint(Id=1) from the Container.LazyDataPoint collection.
			// This behaviour would seem to be necessary 'by design', as a comment in EvictCascadingAction states, "evicts don't
			// cascade to uninitialized collections".
			// If LazyDataPoint(Id=1) is not evicted, it has a status of Loaded, not ReadOnly, and causes the test to fail further
			// down.
			// Another way to get this test to pass is s.Clear().
			await (NHibernateUtil.InitializeAsync(cOrig.LazyDataPoints));
			await (s.EvictAsync(cOrig));
			c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ExistingReadOnlyAfterSetSessionModifiableAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = false;
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ExistingReadOnlyAfterSetSessionModifiableExistingAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			DataPoint lazyDataPointOrig = cOrig.LazyDataPoints.First();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = false;
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			DataPoint lazyDataPoint = await (s.GetAsync<DataPoint>(lazyDataPointOrig.Id));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ExistingReadOnlyAfterSetSessionModifiableExistingEntityReadOnlyAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			DataPoint lazyDataPointOrig = cOrig.LazyDataPoints.First();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = false;
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			DataPoint lazyDataPoint = await (s.GetAsync<DataPoint>(lazyDataPointOrig.Id));
			s.DefaultReadOnly = false;
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(lazyDataPoint);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ExistingReadOnlyAfterSetSessionModifiableProxyExistingAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			DataPoint lazyDataPointOrig = cOrig.LazyDataPoints.First();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = false;
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			DataPoint lazyDataPoint = await (s.LoadAsync<DataPoint>(lazyDataPointOrig.Id));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task ExistingReadOnlyAfterSetSessionModifiableExistingProxyReadOnlyAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			DataPoint lazyDataPointOrig = cOrig.LazyDataPoints.First();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = false;
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			DataPoint lazyDataPoint = await (s.LoadAsync<DataPoint>(lazyDataPointOrig.Id));
			s.DefaultReadOnly = false;
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(lazyDataPoint);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultModifiableWithReadOnlyQueryForEntityAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = await (s.CreateQuery("from Container where id=" + cOrig.Id).SetReadOnly(true).UniqueResultAsync<Container>());
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultReadOnlyWithModifiableQueryForEntityAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = await (s.CreateQuery("from Container where id=" + cOrig.Id).SetReadOnly(false).UniqueResultAsync<Container>());
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultReadOnlyWithQueryForEntityAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = await (s.CreateQuery("from Container where id=" + cOrig.Id).UniqueResultAsync<Container>());
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultModifiableWithQueryForEntityAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = await (s.CreateQuery("from Container where id=" + cOrig.Id).UniqueResultAsync<Container>());
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			await (NHibernateUtil.InitializeAsync(c.ProxyInfo));
			expectedInitializedObjects.Add(c.ProxyInfo);
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			await (NHibernateUtil.InitializeAsync(c.LazyDataPoints));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			await (CheckContainerAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultModifiableWithReadOnlyQueryForCollectionEntitiesAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			DataPoint dp = await (s.CreateQuery("select c.LazyDataPoints from Container c join c.LazyDataPoints where c.Id=" + cOrig.Id).SetReadOnly(true).UniqueResultAsync<DataPoint>());
			Assert.That(s.IsReadOnly(dp), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultReadOnlyWithModifiableFilterCollectionEntitiesAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			IList list = await ((await (s.CreateFilterAsync(c.LazyDataPoints, ""))).SetMaxResults(1).SetReadOnly(false).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = await ((await (s.CreateFilterAsync(c.NonLazyJoinDataPoints, ""))).SetMaxResults(1).SetReadOnly(false).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = await ((await (s.CreateFilterAsync(c.NonLazySelectDataPoints, ""))).SetMaxResults(1).SetReadOnly(false).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultModifiableWithReadOnlyFilterCollectionEntitiesAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			IList list = await ((await (s.CreateFilterAsync(c.LazyDataPoints, ""))).SetMaxResults(1).SetReadOnly(true).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = await ((await (s.CreateFilterAsync(c.NonLazyJoinDataPoints, ""))).SetMaxResults(1).SetReadOnly(true).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = await ((await (s.CreateFilterAsync(c.NonLazySelectDataPoints, ""))).SetMaxResults(1).SetReadOnly(true).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultReadOnlyWithFilterCollectionEntitiesAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			IList list = await ((await (s.CreateFilterAsync(c.LazyDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = await ((await (s.CreateFilterAsync(c.NonLazyJoinDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = await ((await (s.CreateFilterAsync(c.NonLazySelectDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task DefaultModifiableWithFilterCollectionEntitiesAsync()
		{
			Container cOrig = CreateContainer();
			ISet<object> expectedInitializedObjects = new HashSet<object>(new object[]{cOrig, //cOrig.NoProxyInfo,
			cOrig.ProxyInfo, cOrig.NonLazyInfo, //cOrig.NoProxyOwner,
			cOrig.ProxyOwner, cOrig.NonLazyOwner, cOrig.LazyDataPoints.First(), cOrig.NonLazyJoinDataPoints.First(), cOrig.NonLazySelectDataPoints.First()});
			ISet<object> expectedReadOnlyObjects = new HashSet<object>();
			ISession s = OpenSession();
			Assert.That(s.DefaultReadOnly, Is.False);
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(cOrig));
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			await (CheckContainerAsync(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			IList list = await ((await (s.CreateFilterAsync(c.LazyDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = await ((await (s.CreateFilterAsync(c.NonLazyJoinDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = await ((await (s.CreateFilterAsync(c.NonLazySelectDataPoints, ""))).SetMaxResults(1).ListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.CreateQuery("delete from DataPoint").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Container").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Info").ExecuteUpdateAsync());
			await (s.CreateQuery("delete from Owner").ExecuteUpdateAsync());
			await (t.CommitAsync());
			s.Close();
		}

		private async Task CheckContainerAsync(Container c, ISet<object> expectedInitializedObjects, ISet<object> expectedReadOnlyObjects, ISession s)
		{
			await (CheckObjectAsync(c, expectedInitializedObjects, expectedReadOnlyObjects, s));
			if (!expectedInitializedObjects.Contains(c))
			{
				return;
			}

			await (CheckObjectAsync(c.ProxyInfo, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (CheckObjectAsync(c.NonLazyInfo, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (CheckObjectAsync(c.ProxyOwner, expectedInitializedObjects, expectedReadOnlyObjects, s));
			await (CheckObjectAsync(c.NonLazyOwner, expectedInitializedObjects, expectedReadOnlyObjects, s));
			if (NHibernateUtil.IsInitialized(c.LazyDataPoints))
			{
				foreach (DataPoint dp in c.LazyDataPoints)
					await (CheckObjectAsync(dp, expectedInitializedObjects, expectedReadOnlyObjects, s));
				foreach (DataPoint dp in c.NonLazyJoinDataPoints)
					await (CheckObjectAsync(dp, expectedInitializedObjects, expectedReadOnlyObjects, s));
				foreach (DataPoint dp in c.NonLazySelectDataPoints)
					await (CheckObjectAsync(dp, expectedInitializedObjects, expectedReadOnlyObjects, s));
			}
		}

		private async Task CheckObjectAsync(object entityOrProxy, ISet<object> expectedInitializedObjects, ISet<object> expectedReadOnlyObjects, ISession s)
		{
			bool isExpectedToBeInitialized = expectedInitializedObjects.Contains(entityOrProxy);
			bool isExpectedToBeReadOnly = expectedReadOnlyObjects.Contains(entityOrProxy);
			ISessionImplementor si = (ISessionImplementor)s;
			Assert.That(NHibernateUtil.IsInitialized(entityOrProxy), Is.EqualTo(isExpectedToBeInitialized));
			Assert.That(s.IsReadOnly(entityOrProxy), Is.EqualTo(isExpectedToBeReadOnly));
			if (NHibernateUtil.IsInitialized(entityOrProxy))
			{
				object entity = entityOrProxy is INHibernateProxy ? await (((INHibernateProxy)entityOrProxy).HibernateLazyInitializer.GetImplementationAsync(si)) : entityOrProxy;
				Assert.That(entity, Is.Not.Null);
				Assert.That(s.IsReadOnly(entity), Is.EqualTo(isExpectedToBeReadOnly));
			}
		}
	}
}
#endif
