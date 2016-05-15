#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicEntityTest : BugTestCase
	{
		[Test]
		public async Task CanFetchByPropertyAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var l = await (s.CreateQuery("from DynamicEntity de where de.SomeProps.Foo=:fooParam").SetString("fooParam", "Sweden").ListAsync());
					Assert.AreEqual(1, l.Count);
					var props = ((IDictionary)l[0])["SomeProps"];
					Assert.AreEqual("IsCold", ((IDictionary)props)["Bar"]);
				}
			}
		}
	}
}
#endif
