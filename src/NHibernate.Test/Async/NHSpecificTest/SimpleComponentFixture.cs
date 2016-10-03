#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleComponentFixtureAsync : TestCaseAsync
	{
		private DateTime testDateTime = new DateTime(2003, 8, 16);
		private DateTime updateDateTime = new DateTime(2003, 8, 17);
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecific.SimpleComponent.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					// create a new
					SimpleComponent simpleComp = new SimpleComponent();
					simpleComp.Name = "Simple 1";
					simpleComp.Address = "Street 12";
					simpleComp.Date = testDateTime;
					simpleComp.Count = 99;
					simpleComp.Audit.CreatedDate = DateTime.Now;
					simpleComp.Audit.CreatedUserId = "TestCreated";
					simpleComp.Audit.UpdatedDate = DateTime.Now;
					simpleComp.Audit.UpdatedUserId = "TestUpdated";
					await (s.SaveAsync(simpleComp, 10L));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(await (s.LoadAsync(typeof (SimpleComponent), 10L))));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task TestLoadAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					SimpleComponent simpleComp = (SimpleComponent)await (s.LoadAsync(typeof (SimpleComponent), 10L));
					Assert.AreEqual(10L, simpleComp.Key);
					Assert.AreEqual("TestCreated", simpleComp.Audit.CreatedUserId);
					Assert.AreEqual("TestUpdated", simpleComp.Audit.UpdatedUserId);
					await (t.CommitAsync());
				}
		}
	}
}
#endif
