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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1349
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			using(var session=this.OpenSession())
			{
				using(var tran=session.BeginTransaction())
				{
					string name = "fabio";
					string accNum = DateTime.Now.Ticks.ToString(); ;
					Services newServ = new Services();
					newServ.AccountNumber = accNum;
					newServ.Name = name + " person";
					newServ.Type = (new Random()).Next(0, 9).ToString();

					session.Save(newServ);
					tran.Commit();
				}
			}
		}
		protected override void OnTearDown()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					session.Delete("from Services");
					tran.Commit();
				}
			}
		}

		[Test]
		public async Task Can_page_with_formula_propertyAsync()
		{
			using (var session = this.OpenSession())
			{
				using(var tran=session.BeginTransaction())
				{
					IList ret = await (session.CreateCriteria(typeof(Services)).SetMaxResults(5).ListAsync()); //this breaks
					Assert.That(ret.Count,Is.EqualTo(1));
				}
			}
		}
	}
}
