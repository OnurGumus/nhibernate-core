#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1899
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task ShouldNotThrowOnMergeAsync()
		{
			Parent entity;
			using (ISession session = OpenSession())
			{
				entity = await (session.GetAsync<Parent>(1));
				session.Close();
				session.Dispose();
			}

			using (ISession session2 = OpenSession())
			{
				entity = session2.Merge(entity);
			}
		}
	}
}
#endif
