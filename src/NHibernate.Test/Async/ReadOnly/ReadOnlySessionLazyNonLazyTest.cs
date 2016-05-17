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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Container c = s.Load<Container>(cOrig.Id);
			Assert.That(cOrig, Is.SameAs(c));
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.SameAs(c));
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.Refresh(cOrig);
			Assert.That(cOrig, Is.SameAs(c));
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			// NH-specific: The following line is required to evict DataPoint(Id=1) from the Container.LazyDataPoint collection.
			// This behaviour would seem to be necessary 'by design', as a comment in EvictCascadingAction states, "evicts don't
			// cascade to uninitialized collections".
			// If LazyDataPoint(Id=1) is not evicted, it has a status of Loaded, not ReadOnly, and causes the test to fail further
			// down.
			// Another way to get this test to pass is s.Clear().
			NHibernateUtil.Initialize(cOrig.LazyDataPoints);
			s.Evict(cOrig);
			c = await (s.GetAsync<Container>(cOrig.Id));
			Assert.That(cOrig, Is.Not.SameAs(c));
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			// The following check fails if the NH-specific change (above) is not made. More specifically it fails
			// when asserting that the c.LazyDataPoints.First() is ReadOnly
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = false;
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			//expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = false;
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			DataPoint lazyDataPoint = await (s.GetAsync<DataPoint>(lazyDataPointOrig.Id));
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = false;
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			DataPoint lazyDataPoint = await (s.GetAsync<DataPoint>(lazyDataPointOrig.Id));
			s.DefaultReadOnly = false;
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(lazyDataPoint);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = false;
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			DataPoint lazyDataPoint = s.Load<DataPoint>(lazyDataPointOrig.Id);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = false;
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			DataPoint lazyDataPoint = s.Load<DataPoint>(lazyDataPointOrig.Id);
			s.DefaultReadOnly = false;
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			Assert.That(lazyDataPoint, Is.SameAs(c.LazyDataPoints.First()));
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(lazyDataPoint);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = s.CreateQuery("from Container where id=" + cOrig.Id).SetReadOnly(true).UniqueResult<Container>();
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			//expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = s.CreateQuery("from Container where id=" + cOrig.Id).SetReadOnly(false).UniqueResult<Container>();
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			Container c = s.CreateQuery("from Container where id=" + cOrig.Id).UniqueResult<Container>();
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>(new object[]{c, //c.NoProxyInfo,
			c.ProxyInfo, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, //c.getLazyDataPoints(),
			c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			Container c = s.CreateQuery("from Container where id=" + cOrig.Id).UniqueResult<Container>();
			expectedInitializedObjects = new HashSet<object>(new object[]{c, c.NonLazyInfo, //c.NoProxyOwner,
			c.ProxyOwner, c.NonLazyOwner, c.NonLazyJoinDataPoints.First(), c.NonLazySelectDataPoints.First()});
			expectedReadOnlyObjects = new HashSet<object>();
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			//			Assert.That(NHibernateUtil.IsInitialized(c.NoProxyInfo), Is.False);
			//			NHibernateUtil.Initialize(c.NoProxyInfo);
			//			expectedInitializedObjects.Add(c.NoProxyInfo);
			//			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.ProxyInfo), Is.False);
			NHibernateUtil.Initialize(c.ProxyInfo);
			expectedInitializedObjects.Add(c.ProxyInfo);
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			Assert.That(NHibernateUtil.IsInitialized(c.LazyDataPoints), Is.False);
			NHibernateUtil.Initialize(c.LazyDataPoints);
			expectedInitializedObjects.Add(c.LazyDataPoints.First());
			//expectedReadOnlyObjects.Add(c.LazyDataPoints.First());
			CheckContainer(c, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Assert.That(s.DefaultReadOnly, Is.False);
			DataPoint dp = s.CreateQuery("select c.LazyDataPoints from Container c join c.LazyDataPoints where c.Id=" + cOrig.Id).SetReadOnly(true).UniqueResult<DataPoint>();
			Assert.That(s.IsReadOnly(dp), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			IList list = s.CreateFilter(c.LazyDataPoints, "").SetMaxResults(1).SetReadOnly(false).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = s.CreateFilter(c.NonLazyJoinDataPoints, "").SetMaxResults(1).SetReadOnly(false).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = s.CreateFilter(c.NonLazySelectDataPoints, "").SetMaxResults(1).SetReadOnly(false).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			IList list = s.CreateFilter(c.LazyDataPoints, "").SetMaxResults(1).SetReadOnly(true).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = s.CreateFilter(c.NonLazyJoinDataPoints, "").SetMaxResults(1).SetReadOnly(true).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = s.CreateFilter(c.NonLazySelectDataPoints, "").SetMaxResults(1).SetReadOnly(true).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			IList list = s.CreateFilter(c.LazyDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = s.CreateFilter(c.NonLazyJoinDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			list = s.CreateFilter(c.NonLazySelectDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
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
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
			s.DefaultReadOnly = true;
			Assert.That(s.DefaultReadOnly, Is.True);
			CheckContainer(cOrig, expectedInitializedObjects, expectedReadOnlyObjects, s);
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
			IList list = s.CreateFilter(c.LazyDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = s.CreateFilter(c.NonLazyJoinDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			list = s.CreateFilter(c.NonLazySelectDataPoints, "").SetMaxResults(1).List();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(s.IsReadOnly(list[0]), Is.False);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			s.CreateQuery("delete from DataPoint").ExecuteUpdate();
			s.CreateQuery("delete from Container").ExecuteUpdate();
			s.CreateQuery("delete from Info").ExecuteUpdate();
			s.CreateQuery("delete from Owner").ExecuteUpdate();
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
