#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleComponentFixture : TestCase
	{
		[Test]
		public async Task TestLoadAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					SimpleComponent simpleComp = (SimpleComponent)s.Load(typeof (SimpleComponent), 10L);
					Assert.AreEqual(10L, simpleComp.Key);
					Assert.AreEqual("TestCreated", simpleComp.Audit.CreatedUserId);
					Assert.AreEqual("TestUpdated", simpleComp.Audit.UpdatedUserId);
					await (t.CommitAsync());
				}
		}
	}
}
#endif
