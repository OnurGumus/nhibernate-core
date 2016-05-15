#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2982
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SimpleExpressionWithProxyAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = await (session.LoadAsync<Entity>(1));
					var restriction = Restrictions.Eq("A", a);
					Assert.AreEqual("A = Entity#1", restriction.ToString());
				}
		}
	}
}
#endif
