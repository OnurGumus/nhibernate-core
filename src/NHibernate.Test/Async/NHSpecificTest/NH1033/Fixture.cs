#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1033
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					var animal0 = new Animal();
					var animal1 = new Reptile();
					animal0.SerialNumber = "00001";
					animal1.SerialNumber = "00002";
					animal1.BodyTemperature = 34;
					await (session.SaveAsync(animal0));
					await (session.SaveAsync(animal1));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Animal"));
					await (session.DeleteAsync("from Reptile"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseClassConstraintAsync()
		{
			using (ISession session = OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Animal), "a").Add(Property.ForName("a.class").Eq(typeof (Animal)));
				var results = await (crit.ListAsync<Animal>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(typeof (Animal), await (NHibernateUtil.GetClassAsync(results[0])));
			}
		}
	}
}
#endif
