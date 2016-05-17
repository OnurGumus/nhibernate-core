#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH401
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task MergeAsync()
		{
			object clubId;
			using (ISession s = OpenSession())
			{
				Club club1 = new Club();
				clubId = await (s.SaveAsync(club1));
				await (s.FlushAsync());
			}

			Clubmember mem = new Clubmember();
			ISession sess2 = OpenSession();
			mem.Club = (Club)sess2.Get(typeof (Club), clubId);
			sess2.Close();
			ISession sess = OpenSession();
			mem.Expirydate = DateTime.Now.AddYears(1);
			mem.Joindate = DateTime.Now;
			sess.Merge(mem);
			await (sess.FlushAsync());
			sess.Close();
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from System.Object"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
