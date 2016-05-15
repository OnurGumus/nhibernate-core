#if NET_4_5
using System;
using System.Linq;
using NHibernate.Linq;
using NHibernate.Collection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3480
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				using (session.BeginTransaction())
				{
					var result =
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e;
					var entity = result.Single();
					await (NHibernateUtil.InitializeAsync(entity.Children));
				}
			}
		}
	}
}
#endif
