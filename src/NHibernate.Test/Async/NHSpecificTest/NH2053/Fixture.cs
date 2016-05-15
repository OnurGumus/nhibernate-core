#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2053
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task JoinedSubClass_FilterAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					session.EnableFilter("talkableFilter").SetParameter("talkable", true);
					var snoopy = await (session.QueryOver<Dog>().Where(x => x.Name == "Snoopy").SingleOrDefaultAsync());
					Assert.AreEqual(null, snoopy); // there are no talking dog named Snoopy.
					var jake = await (session.QueryOver<Dog>().Where(x => x.Name == "Jake the dog").SingleOrDefaultAsync());
					Assert.AreNotEqual(null, jake);
					Assert.AreEqual("Jake the dog", jake.Name);
					var kitty = await (session.QueryOver<Cat>().Where(x => x.Name == "Kitty").SingleOrDefaultAsync());
					Assert.AreNotEqual(null, kitty);
					Assert.AreEqual("Kitty", kitty.Name);
				}
			}
		}
	}
}
#endif
