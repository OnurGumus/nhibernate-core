#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1689
{
	using System.Collections.Generic;
	using Dialect;
	using NUnit.Framework;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task ShouldBeAbleToCallGenericMethodAsync()
		{
			using (ISession session = this.OpenSession())
			{
				DomainClass entity = await (session.LoadAsync<DomainClass>(1));
				IList<string> inputStrings = entity.GetListOfTargetType<string>("arg");
				Assert.That(inputStrings.Count == 0);
			}
		}
	}
}
#endif
