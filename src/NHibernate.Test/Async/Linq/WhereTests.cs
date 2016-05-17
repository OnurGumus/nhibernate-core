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

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WhereTests : LinqTestCase
	{
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
	}
}
#endif
