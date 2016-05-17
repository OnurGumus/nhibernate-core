#if NET_4_5
using System;
using System.Collections;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Test;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade.Circle
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiPathCircleCascadeTest : TestCase
	{
		[Test]
		public async Task MergeEntityWithNonNullableTransientEntityAsync()
		{
			Route route = await (this.GetUpdatedDetachedEntityAsync());
			Node node = route.Nodes.First();
			route.Nodes.Remove(node);
			Route routeNew = new Route();
			routeNew.Name = "new route";
			routeNew.Nodes.Add(node);
			node.Route = routeNew;
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						session.Merge(node);
						Assert.Fail("should have thrown an exception");
					}
					catch (Exception ex)
					{
						Assert.That(ex, Is.TypeOf(typeof (TransientObjectException)));
					//					if (((SessionImplementor)session).Factory.Settings.isCheckNullability() ) {
					//						assertTrue( ex instanceof TransientObjectException );
					//					}
					//					else {
					//						assertTrue( ex instanceof JDBCException );
					//					}
					}
					finally
					{
						transaction.Rollback();
					}
				}
		}

		[Test]
		public async Task MergeEntityWithNonNullableEntityNullAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			Node node = route.Nodes.First();
			route.Nodes.Remove(node);
			node.Route = null;
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						session.Merge(node);
						Assert.Fail("should have thrown an exception");
					}
					catch (Exception ex)
					{
						Assert.That(ex, Is.TypeOf(typeof (PropertyValueException)));
					//					if ( ( ( SessionImplementor ) s ).getFactory().getSettings().isCheckNullability() ) {
					//						assertTrue( ex instanceof PropertyValueException );
					//					}
					//					else {
					//						assertTrue( ex instanceof JDBCException );
					//					}
					}
					finally
					{
						transaction.Rollback();
					}
				}
		}

		public async Task MergeEntityWithNonNullablePropSetToNullAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			Node node = route.Nodes.First();
			node.Name = null;
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						session.Merge(route);
						Assert.Fail("should have thrown an exception");
					}
					catch (Exception ex)
					{
						Assert.That(ex, Is.TypeOf(typeof (PropertyValueException)));
					//					if ( ( ( SessionImplementor ) s ).getFactory().getSettings().isCheckNullability() ) {
					//						assertTrue( ex instanceof PropertyValueException );
					//					}
					//					else {
					//						assertTrue( ex instanceof JDBCException );
					//					}
					}
					finally
					{
						transaction.Rollback();
					}
				}
		}

		[Test]
		public async Task MergeRouteAsync()
		{
			Route route = await (this.GetUpdatedDetachedEntityAsync());
			ClearCounts();
			ISession s = base.OpenSession();
			s.BeginTransaction();
			s.Merge(route);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(1);
			s = base.OpenSession();
			s.BeginTransaction();
			route = await (s.GetAsync<Route>(route.RouteId));
			CheckResults(route, true);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergePickupNodeAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			Node pickupNode = route.Nodes.First(n => n.Name == "pickupNodeB");
			pickupNode = (Node)s.Merge(pickupNode);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			route = await (s.GetAsync<Route>(route.RouteId));
			CheckResults(route, false);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergeDeliveryNodeAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			Node deliveryNode = route.Nodes.First(n => n.Name == "deliveryNodeB");
			deliveryNode = (Node)s.Merge(deliveryNode);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			route = await (s.GetAsync<Route>(route.RouteId));
			CheckResults(route, false);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergeTourAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			Tour tour = (Tour)s.Merge(route.Nodes.First().Tour);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			route = await (s.GetAsync<Route>(route.RouteId));
			CheckResults(route, false);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergeTransportAsync()
		{
			Route route = await (GetUpdatedDetachedEntityAsync());
			ClearCounts();
			ISession s = OpenSession();
			s.BeginTransaction();
			Node node = route.Nodes.First();
			Transport transport = null;
			if (node.PickupTransports.Count == 1)
				transport = node.PickupTransports.First();
			else
				transport = node.DeliveryTransports.First();
			transport = (Transport)s.Merge(transport);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertInsertCount(4);
			AssertUpdateCount(0);
			s = OpenSession();
			s.BeginTransaction();
			route = await (s.GetAsync<Route>(route.RouteId));
			CheckResults(route, false);
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		private async Task<Route> GetUpdatedDetachedEntityAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			Route route = new Route();
			route.Name = "routeA";
			await (s.SaveAsync(route));
			await (s.Transaction.CommitAsync());
			s.Close();
			route.Name = "new routeA";
			route.TransientField = "sfnaouisrbn";
			Tour tour = new Tour();
			tour.Name = "tourB";
			Transport transport = new Transport();
			transport.Name = "transportB";
			Node pickupNode = new Node();
			pickupNode.Name = "pickupNodeB";
			Node deliveryNode = new Node();
			deliveryNode.Name = "deliveryNodeB";
			pickupNode.Route = route;
			pickupNode.Tour = tour;
			pickupNode.PickupTransports.Add(transport);
			pickupNode.TransientField = "pickup node aaaaaaaaaaa";
			deliveryNode.Route = route;
			deliveryNode.Tour = tour;
			deliveryNode.DeliveryTransports.Add(transport);
			deliveryNode.TransientField = "delivery node aaaaaaaaa";
			tour.Nodes.Add(pickupNode);
			tour.Nodes.Add(deliveryNode);
			route.Nodes.Add(pickupNode);
			route.Nodes.Add(deliveryNode);
			transport.PickupNode = pickupNode;
			transport.DeliveryNode = deliveryNode;
			transport.TransientField = "aaaaaaaaaaaaaa";
			return route;
		}
	}
}
#endif
