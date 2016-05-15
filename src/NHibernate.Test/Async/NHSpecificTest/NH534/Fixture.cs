#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH534
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			Parent p;
			Child c;
			using (ISession s = OpenSession())
			{
				p = new Parent();
				await (s.SaveAsync(p));
				c = new Child(p);
				await (s.SaveAsync(c));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(c));
				await (s.DeleteAsync(p));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
