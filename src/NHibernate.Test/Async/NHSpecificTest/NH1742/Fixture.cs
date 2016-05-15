#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1742
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugTestAsync()
		{
			IQuery query = session.CreateQuery("SELECT e FROM Event e " + " inner join fetch e.descriptions d " + " WHERE (e.SendedBy in( :dev)) " + " AND (e.Date >= :from) " + " AND (e.Date <= :to)" + " ORDER BY d.Value");
			var devices = new List<Device>{device};
			query.SetParameterList("dev", devices).SetDateTime("from", date).SetDateTime("to", date.AddMonths(1));
			Assert.AreEqual(1, (await (query.ListAsync<Event>())).Count);
		}

		[Test]
		public async Task WorkingTestAsync()
		{
			IQuery query = session.CreateQuery("SELECT e FROM Event e " + " inner join fetch e.descriptions d " + " WHERE (e.Date >= :from) " + " AND (e.Date <= :to)" + " AND (e.SendedBy in( :dev)) " + " ORDER BY d.Value");
			var devices = new List<Device>{device};
			query.SetParameterList("dev", devices).SetDateTime("from", date).SetDateTime("to", date.AddMonths(1));
			Assert.AreEqual(1, (await (query.ListAsync<Event>())).Count);
		}

		[Test]
		public async Task NH2213Async()
		{
			IQuery query = session.CreateQuery("SELECT e FROM Event e " + " inner join fetch e.descriptions d " + " WHERE (e.SendedBy in( :dev)) " + " AND (e.Date >= :from) " + " AND (e.Date <= :to)" + " ORDER BY d.Value");
			var devices = new List<Device>();
			devices.Add(device);
			query.SetParameterList("dev", devices);
			query.SetDateTime("from", date);
			query.SetDateTime("to", date.AddMonths(1));
			Assert.AreEqual(1, (await (query.ListAsync<Event>())).Count);
		}
	}
}
#endif
