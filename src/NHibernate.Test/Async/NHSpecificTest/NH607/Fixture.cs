#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH607
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
				await (session.SaveAsync(pac));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object o"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
