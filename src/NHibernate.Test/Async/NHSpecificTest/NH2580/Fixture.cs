#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2580
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class MyClass
		{
		}

		[Test]
		public async Task WhenPersisterNotFoundShouldThrowAMoreExplicitExceptionAsync()
		{
			using (var s = OpenSession())
			{
				var exeption = Assert.ThrowsAsync<HibernateException>(async () => await (s.GetAsync<MyClass>(1)));
				Assert.That(exeption.Message.ToLowerInvariant(), Is.StringContaining("possible cause"));
			}
		}
	}
}
#endif
