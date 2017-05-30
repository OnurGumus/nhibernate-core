﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class SimpleComponentFixtureAsync : TestCase
	{
		private DateTime testDateTime = new DateTime(2003, 8, 16);
		private DateTime updateDateTime = new DateTime(2003, 8, 17);

		protected override IList Mappings
		{
			get { return new string[] {"NHSpecific.SimpleComponent.hbm.xml"}; }
		}

		protected override void OnSetUp()
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


				s.Save(simpleComp, 10L);

				t.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete(s.Load(typeof(SimpleComponent), 10L));
				s.Flush();
			}
		}


		[Test]
		public async Task TestLoadAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				SimpleComponent simpleComp = (SimpleComponent) await (s.LoadAsync(typeof(SimpleComponent), 10L, CancellationToken.None));

				Assert.AreEqual(10L, simpleComp.Key);
				Assert.AreEqual("TestCreated", simpleComp.Audit.CreatedUserId);
				Assert.AreEqual("TestUpdated", simpleComp.Audit.UpdatedUserId);

				await (t.CommitAsync(CancellationToken.None));
			}
		}
	}
}