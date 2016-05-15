#if NET_4_5
using System;
using System.Linq;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3252
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task VerifyThatWeCanSaveAndLoadAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Note{Text = new String('0', 9000)}));
					await (transaction.CommitAsync());
				}

			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var note = session.Query<Note>().First();
					Assert.AreEqual(9000, note.Text.Length);
				}
		}
	}
}
#endif
