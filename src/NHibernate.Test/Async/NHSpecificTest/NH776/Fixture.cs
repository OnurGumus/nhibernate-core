#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH776
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ProxiedOneToOneTestAsync()
		{
			//Instantiate and setup associations (all needed to generate the error);
			A a = new A(1, "aaa");
			try
			{
				using (ISession session = sessions.OpenSession())
				{
					await (session.SaveAsync(a));
					await (session.FlushAsync());
				}

				using (ISession session = sessions.OpenSession())
				{
					A loadedA = (A)await (session.LoadAsync(typeof (A), 1));
					Assert.IsNull(loadedA.NotProxied);
					Assert.IsNull(loadedA.Proxied, "one-to-one to proxied types not handling missing associated classes correctly (as null)");
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync(a));
					await (session.FlushAsync());
				}
			}
		}
	}
}
#endif
