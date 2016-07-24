#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Properties
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicEntityTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					var props = new Dictionary<string, object>();
					props["Foo"] = "Sweden";
					props["Bar"] = "IsCold";
					await (s.SaveAsync("DynamicEntity", new Dictionary<string, object>{{"SomeProps", props}, }));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from DynamicEntity"));
					await (tx.CommitAsync());
				}
			}
		}

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
