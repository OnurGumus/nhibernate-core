#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1788
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseSqlTimestampWithDynamicInsertAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Name = "hi"}));
					await (tx.CommitAsync());
				}

			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var person = await (session.GetAsync<Person>(1));
					person.Name = "other";
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync(await (session.GetAsync<Person>(1))));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
