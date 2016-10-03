#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1349
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					string name = "fabio";
					string accNum = DateTime.Now.Ticks.ToString();
					;
					Services newServ = new Services();
					newServ.AccountNumber = accNum;
					newServ.Name = name + " person";
					newServ.Type = (new Random()).Next(0, 9).ToString();
					await (session.SaveAsync(newServ));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Services"));
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public async Task Can_page_with_formula_propertyAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					IList ret = await (session.CreateCriteria(typeof (Services)).SetMaxResults(5).ListAsync()); //this breaks
					Assert.That(ret.Count, Is.EqualTo(1));
				}
			}
		}
	}
}
#endif
