#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH623
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhereAttributesOnBagsAsync()
		{
			IList result;
			Document d;
			result = await (session.CreateCriteria(typeof (Document)).ListAsync());
			d = result[0] as Document;
			// collection is lazy loaded an so it is also filtered so we will get here one element
			Assert.AreEqual(1, d.Pages.Count);
			session.Clear();
			result = await (session.CreateCriteria(typeof (Document)).SetFetchMode("Pages", FetchMode.Join).ListAsync());
			d = result[0] as Document;
			// this assertion fails because if the collection is eager fetched it will contain all elements and will ignore the where clause.
			Assert.AreEqual(1, d.Pages.Count);
		}
	}
}
#endif
