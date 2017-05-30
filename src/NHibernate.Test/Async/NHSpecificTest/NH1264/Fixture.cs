﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1264
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new[] {"NHSpecificTest.NH1264.Passenger.hbm.xml", "NHSpecificTest.NH1264.Reservation.hbm.xml",}; }
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();
			using (ISession s = OpenSession())
			{
				s.Delete("from Reservation r");
				s.Delete("from Passenger p");
				s.Flush();
			}
		}

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
			await (s.SaveAsync(mickey, CancellationToken.None));

			var reservation = new Reservation();
			reservation.ConfirmationNumber = "11111111111111";
			reservation.Passengers.Add(mickey);
			mickey.Reservation = reservation;
			await (s.SaveAsync(reservation, CancellationToken.None));

			await (t.CommitAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();

			DetachedCriteria dc = DetachedCriteria.For<Reservation>().SetFetchMode("Passengers", FetchMode.Eager);

			dc.CreateCriteria("Passengers").Add(Property.ForName("FrequentFlyerNumber").Eq("1234"));

			IList<Reservation> results = await (dc.GetExecutableCriteria(s).ListAsync<Reservation>(CancellationToken.None));

			s.Close();

			Assert.AreEqual(1, results.Count);
			foreach (var r in results)
			{
				Assert.AreEqual(1, r.Passengers.Count);
			}

			s = OpenSession();
			t = s.BeginTransaction();

			await (s.DeleteAsync(reservation, CancellationToken.None));

			await (t.CommitAsync(CancellationToken.None));
			s.Close();
		}
	}
}