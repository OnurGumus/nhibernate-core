#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Engine.Query;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WhereTestsAsync : LinqTestCaseAsync
	{
		[Test(Description = "NH-3261")]
		public async Task UsersWithStringContainsAndNotNullNameHQLAsync()
		{
			var users = await (session.CreateQuery("from User u where (case when u.Name is null then 'false' else (case when u.Name LIKE '%yend%' then 'true' else 'false' end) end) = 'true'").ListAsync<User>());
			Assert.That(users.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task TimesheetsWithCollectionContainsAsync()
		{
			var entry = await (session.GetAsync<TimesheetEntry>(1));
			var timesheet = (
				from sheet in db.Timesheets
				where sheet.Entries.Contains(entry)select sheet).Single();
			Assert.That(timesheet.Id, Is.EqualTo(2));
		}

		[Test]
		public async Task TimesheetsWithCollectionNotContainsAsync()
		{
			var entry = await (session.GetAsync<TimesheetEntry>(1));
			var query = (
				from sheet in db.Timesheets
				where !sheet.Entries.Contains(entry)select sheet).ToList();
			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task TimesheetsWithEnumerableContainsAsync()
		{
			var user = await (session.GetAsync<User>(1));
			var query = (
				from sheet in db.Timesheets
				where sheet.Users.Contains(user)select sheet).ToList();
			Assert.That(query.Count, Is.EqualTo(2));
		}

		private static List<object[]> CanUseCompareInQueryDataSource()
		{
			return new List<object[]>{// The full set of operators over strings.
			TestRow(p => p.Name.ToLower().CompareTo("konbu") < 0, 30, false), TestRow(p => p.Name.ToLower().CompareTo("konbu") <= 0, 31, false), TestRow(p => p.Name.ToLower().CompareTo("konbu") == 0, 1, false), TestRow(p => p.Name.ToLower().CompareTo("konbu") != 0, 76, false), TestRow(p => p.Name.ToLower().CompareTo("konbu") >= 0, 47, false), TestRow(p => p.Name.ToLower().CompareTo("konbu") > 0, 46, false), // Some of the above with the constant zero as first operator (needs to inverse the operator).
			TestRow(p => 0 <= p.Name.ToLower().CompareTo("konbu"), 47, false), TestRow(p => 0 == p.Name.ToLower().CompareTo("konbu"), 1, false), TestRow(p => 0 > p.Name.ToLower().CompareTo("konbu"), 30, false), // Over integers.
			TestRow(p => p.UnitsInStock.CompareTo(13) < 0, 15, false), TestRow(p => p.UnitsInStock.CompareTo(13) >= 0, 62, false), // Over floats.
			TestRow(p => p.ShippingWeight.CompareTo((float)4.98) <= 0, 17, false), TestRow(p => p.ShippingWeight.CompareTo((float)4.98) <= 0, 17, false), // Over nullable decimals.
			TestRow(p => p.UnitPrice.Value.CompareTo((decimal)14.00) <= 0, 24, false), TestRow(p => 0 >= p.UnitPrice.Value.CompareTo((decimal)14.00), 24, false), // Over nullable DateTime.
			TestRow(p => p.OrderLines.Any(o => o.Order.ShippingDate.Value.CompareTo(DateTime.Now) <= 0), 77, false), TestRow(p => p.OrderLines.Any(o => 0 >= o.Order.ShippingDate.Value.CompareTo(DateTime.Now)), 77, false), };
		}

		private static object[] TestRow(Expression<Func<Product, bool>> expression, int expectedCount, bool expectCase)
		{
			return new object[]{expression, expectedCount, expectCase};
		}
	}
}
#endif
