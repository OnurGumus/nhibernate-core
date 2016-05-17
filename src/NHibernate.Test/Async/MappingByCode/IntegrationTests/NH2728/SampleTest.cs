#if NET_4_5
using System;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH2728
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : TestCaseMappingByCode
	{
		[Test]
		public async Task ShouldBeAbleToGetFromToyToAnimalsAsync()
		{
			using (ISession session = this.OpenSession())
			{
				var toy1 = await (session.GetAsync<Toy>(1));
				Assert.AreEqual(1, toy1.Id);
				Assert.AreEqual(3, toy1.Animals.Count);
			}
		}
	}
}
#endif
