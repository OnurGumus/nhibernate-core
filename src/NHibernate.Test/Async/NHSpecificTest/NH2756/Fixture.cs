#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2756
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task Saving_SetOfComponentsWithFormulaColumn_ShouldWorkAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var file = new File{Filename = "MyFilename"};
					var document = new Document();
					document.Files.Add(new DocumentFile{Description = "MyDescription", File = file});
					await (session.SaveAsync(file));
					await (session.SaveAsync(document));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Document"));
					await (session.DeleteAsync("from File"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
