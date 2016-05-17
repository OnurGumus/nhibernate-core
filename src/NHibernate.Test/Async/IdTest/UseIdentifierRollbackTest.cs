#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UseIdentifierRollbackTest : TestCase
	{
		public async Task SimpleRollbackAsync()
		{
			ISession session = OpenSession();
			ITransaction t = session.BeginTransaction();
			Product prod = new Product();
			Assert.IsNull(prod.Name);
			await (session.PersistAsync(prod));
			await (session.FlushAsync());
			Assert.IsNotNull(prod.Name);
			t.Rollback();
			session.Close();
		}
	}
}
#endif
