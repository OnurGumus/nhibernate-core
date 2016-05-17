#if NET_4_5
using NUnit.Framework;
using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1064
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task JoinFetchAsync()
		{
			TypeA a1;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					a1 = new TypeA("a1");
					a1.C = new TypeC("c1", "c1");
					await (s.SaveAsync(a1.C));
					await (s.SaveAsync(a1));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					ICriteria crit = s.CreateCriteria(typeof (TypeA)).SetFetchMode("Bs", FetchMode.Join).SetFetchMode("C", FetchMode.Join);
					// According to the issue description, the following line
					// would have thown an NHibernate.ADOException before the fix
					IList result = crit.List();
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual(a1.Id, (result[0] as TypeA).Id);
					Assert.AreEqual(a1.Name, (result[0] as TypeA).Name);
					Assert.AreEqual(a1.C.Id, (result[0] as TypeA).C.Id);
					Assert.AreEqual(a1.C.Name, (result[0] as TypeA).C.Name);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
