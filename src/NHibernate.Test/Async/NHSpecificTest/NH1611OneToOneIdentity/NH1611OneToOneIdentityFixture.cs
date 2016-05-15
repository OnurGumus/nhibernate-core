#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1611OneToOneIdentity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1611OneToOneIdentityFixture : BugTestCase
	{
		[Test]
		public async Task CanQueryOneToOneWithCompositeIdAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					ICriteria criteria = s.CreateCriteria(typeof (Primary));
					IList<Primary> list = await (criteria.ListAsync<Primary>());
					Assert.AreEqual("blarg", list[0].Description);
					Assert.AreEqual("nuts", list[0].Adjunct.AdjunctDescription);
				}
			}
		}
	}
}
#endif
