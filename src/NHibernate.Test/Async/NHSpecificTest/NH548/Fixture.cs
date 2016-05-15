#if NET_4_5
using System;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH548
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ParentPropertyOnCacheHitAsync()
		{
			if (cfg.Properties[Environment.CacheProvider] == null)
			{
				Assert.Ignore("Only applicable when a cache provider is enabled");
			}

			// make a new MainObject
			MainObject main = new MainObject();
			main.Name = "Parent";
			main.Component.Note = "Component";
			// save it to the DB
			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(main));
				await (session.FlushAsync());
			}

			// check parent
			Assert.IsNotNull(main.Component.Parent, "component parent null (saved)");
			MainObject getMain;
			using (ISession session = OpenSession())
			{
				getMain = (MainObject)await (session.GetAsync(main.GetType(), main.ID));
				session.Clear();
				Assert.IsNotNull(getMain.Component.Parent, "component parent null (cache miss)");
			}

			using (ISession session = OpenSession())
			{
				getMain = (MainObject)await (session.GetAsync(main.GetType(), main.ID));
				Assert.IsNotNull(getMain.Component.Parent, "component parent null (cache hit)");
				await (session.DeleteAsync(getMain));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
