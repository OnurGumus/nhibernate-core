﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH607
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH607"; }
		}

		[Test]
		public async Task TestAsync()
		{
			PackageParty participant = new PackageParty();
			Package pac = new Package();

			PackageItem packageItem = new PackageItem();
			pac.PackageItems.Add(packageItem);
			packageItem.Package = pac;

			PPP packagePartyParticipant = new PPP();

			packagePartyParticipant.PackageItem = packageItem;
			packagePartyParticipant.PackageParty = participant;

			// make the relation bi-directional
			participant.ParticipatingPackages.Add(packagePartyParticipant);
			packageItem.PackagePartyParticipants.Add(packagePartyParticipant);

			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(pac, CancellationToken.None));
				await (session.FlushAsync(CancellationToken.None));
			}

			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object o", CancellationToken.None));
				await (session.FlushAsync(CancellationToken.None));
			}
		}
	}
}