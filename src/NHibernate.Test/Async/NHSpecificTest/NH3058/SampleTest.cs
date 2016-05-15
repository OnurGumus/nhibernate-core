#if NET_4_5
using NHibernate.Context;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3058
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task MethodShouldLoadLazyPropertyAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var book = await (s.LoadAsync<DomainClass>(1));
					Assert.False(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")));
					string value = book.LoadLazyProperty();
					Assert.That(value, Is.EqualTo("Some text"));
					Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(book, "ALotOfText")), Is.True);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
