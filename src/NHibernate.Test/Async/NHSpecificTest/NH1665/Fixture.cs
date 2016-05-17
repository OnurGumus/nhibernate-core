#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1665
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SupportsHibernateQuotingSequenceNameAsync()
		{
			ISession session = OpenSession();
			session.BeginTransaction();
			var e = new MyEntity{Name = "entity-1"};
			await (session.SaveAsync(e));
			Assert.AreEqual(1, (int)session.GetIdentifier(e));
			await (session.DeleteAsync(e));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
