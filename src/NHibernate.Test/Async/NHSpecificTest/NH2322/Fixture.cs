#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Event;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2322
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ShouldNotThrowWhenCommitingATransactionAsync()
		{
			int id;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var p = new Person{Name = "inserted name"};
					await (s.SaveAsync(p));
					id = p.Id;
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var p = await (s.GetAsync<Person>(id));
					p.Name = "changing the name...";
					Assert.That(async delegate ()
					{
						await (t.CommitAsync());
					}

					, Throws.Nothing);
				}
		}
	}
}
#endif
