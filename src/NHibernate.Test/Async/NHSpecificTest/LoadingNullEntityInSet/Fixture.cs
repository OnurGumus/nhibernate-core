#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.LoadingNullEntityInSet
{
	using System.Collections;
	using Mapping;
	using NUnit.Framework;
	using SqlCommand;
	using Type;
	using TestCase = NHibernate.Test.TestCase;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task CanHandleNullEntityInListAsync()
		{
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					Employee e = new Employee();
					PrimaryProfession ppc = new PrimaryProfession();
					await (sess.SaveAsync(e));
					await (sess.SaveAsync(ppc));
					await (sess.FlushAsync());
					WantedProfession wanted = new WantedProfession();
					wanted.Id = 15;
					wanted.Employee = e;
					wanted.PrimaryProfession = ppc;
					await (sess.SaveAsync(wanted));
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
			{
				ICriteria criteria = sess.CreateCriteria(typeof (Employee));
				criteria.CreateCriteria("Primaries", JoinType.LeftOuterJoin);
				criteria.CreateCriteria("Secondaries", JoinType.LeftOuterJoin);
				await (criteria.ListAsync());
			}

			using (ISession sess = OpenSession())
			{
				await (sess.DeleteAsync("from WantedProfession"));
				await (sess.FlushAsync());
				await (sess.DeleteAsync("from PrimaryProfession"));
				await (sess.FlushAsync());
				await (sess.DeleteAsync("from Employee"));
				await (sess.FlushAsync());
			}
		}
	}
}
#endif
