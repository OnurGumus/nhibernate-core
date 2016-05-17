#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1264
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task EagerFetchAnomalyAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			var mickey = new Passenger();
			mickey.Name = new Name();
			mickey.Name.First = "Mickey";
			mickey.Name.Last = "Mouse";
			mickey.FrequentFlyerNumber = "1234";
			await (s.SaveAsync(mickey));
			var reservation = new Reservation();
			reservation.ConfirmationNumber = "11111111111111";
			reservation.Passengers.Add(mickey);
			mickey.Reservation = reservation;
			await (s.SaveAsync(reservation));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			DetachedCriteria dc = DetachedCriteria.For<Reservation>().SetFetchMode("Passengers", FetchMode.Eager);
			dc.CreateCriteria("Passengers").Add(Property.ForName("FrequentFlyerNumber").Eq("1234"));
			IList<Reservation> results = dc.GetExecutableCriteria(s).List<Reservation>();
			s.Close();
			Assert.AreEqual(1, results.Count);
			foreach (var r in results)
			{
				Assert.AreEqual(1, r.Passengers.Count);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(reservation));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
