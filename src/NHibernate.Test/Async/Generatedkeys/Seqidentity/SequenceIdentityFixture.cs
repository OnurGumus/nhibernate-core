#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Generatedkeys.Seqidentity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceIdentityFixture : TestCase
	{
		[Test]
		public async Task SequenceIdentityGeneratorAsync()
		{
			ISession session = OpenSession();
			session.BeginTransaction();
			var e = new MyEntity{Name = "entity-1"};
			await (session.SaveAsync(e));
			// this insert should happen immediately!
			Assert.AreEqual(1, e.Id, "id not generated through forced insertion");
			await (session.DeleteAsync(e));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
