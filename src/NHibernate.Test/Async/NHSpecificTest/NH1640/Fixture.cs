#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1640
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task FetchJoinShouldNotReturnProxyTestAsync()
		{
			int savedId;
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var sub = new Entity{Id = 2, Name = "Child 2"};
					savedId = (int)await (session.SaveAsync(new Entity{Id = 1, Name = "Parent 1", Child = sub}));
					await (tx.CommitAsync());
				}
			}

			using (IStatelessSession session = sessions.OpenStatelessSession())
			{
				var parent = session.CreateQuery("from Entity p join fetch p.Child where p.Id=:pId").SetInt32("pId", savedId).UniqueResult<Entity>();
				Assert.That(parent.Child, Is.TypeOf(typeof (Entity)));
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Entity"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
