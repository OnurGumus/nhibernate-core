#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1092
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CountHasUniqueResultAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Subscriber1{Username = "u11"}));
					await (s.SaveAsync(new Subscriber1{Username = "u12"}));
					await (s.SaveAsync(new Subscriber1{Username = "u13"}));
					await (s.SaveAsync(new Subscriber2{Username = "u21"}));
					await (s.SaveAsync(new Subscriber2{Username = "u22"}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var count = await (s.CreateQuery("select count(*) from SubscriberAbstract SA where SA.Username like :username").SetString("username", "u%").UniqueResultAsync<long>());
					Assert.That(count, Is.EqualTo(5));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from SubscriberAbstract").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
