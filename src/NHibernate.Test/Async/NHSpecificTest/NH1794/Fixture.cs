#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1794
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanQueryOnCollectionThatAppearsOnlyInTheMappingAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.CreateQuery("select p.Name, c.Name from Person p join p.Children c").ListAsync());
			}
		}

		[Test]
		public async Task CanQueryOnPropertyThatOnlyShowsUpInMapping_AsAccessNoneAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.CreateQuery("from Person p where p.UpdatedAt is null").ListAsync());
			}
		}
	}
}
#endif
