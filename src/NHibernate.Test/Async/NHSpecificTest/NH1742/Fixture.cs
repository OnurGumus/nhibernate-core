#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1742
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private ISession session;
		private ITransaction transaction;
		private Device device;
		private DateTime date = new DateTime(2000, 1, 1);
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			session = OpenSession();
			transaction = session.BeginTransaction();
			device = new Device();
			await (session.SaveAsync(device));
			var ev = new Event{Date = date, SendedBy = device};
			await (session.SaveAsync(ev));
			var d = new Description{Event = ev, Value = "Test", LanguageID = "it"};
			await (session.SaveAsync(d));
			IFilter f = session.EnableFilter("LanguageFilter").SetParameter("LanguageID", "it");
			f.Validate();
		}

		protected override async Task OnTearDownAsync()
		{
			transaction.Rollback();
			session.Close();
			await (base.OnTearDownAsync());
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect as MsSql2000Dialect != null;
		}

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
