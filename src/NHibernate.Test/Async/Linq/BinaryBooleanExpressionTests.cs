#if NET_4_5
using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BinaryBooleanExpressionTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task TimesheetsWithEqualsTrueAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where timesheet.Entries.Any()select timesheet).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task NegativeTimesheetsWithEqualsTrueAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where !timesheet.Entries.Any()select timesheet).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task TimesheetsWithEqualsFalseAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where timesheet.Entries.Any() == false
				select timesheet).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task NegativeTimesheetsWithEqualsFalseAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where !timesheet.Entries.Any() == false
				select timesheet).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task TimesheetsWithNotEqualsTrueAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where timesheet.Entries.Any() != true
				select timesheet).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task NegativeTimesheetsWithNotEqualsTrueAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where !timesheet.Entries.Any() != true
				select timesheet).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task TimesheetsWithNotEqualsFalseAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where timesheet.Entries.Any() != false
				select timesheet).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task NegativeTimesheetsWithNotEqualsFalseAsync()
		{
			var query = await ((
				from timesheet in db.Timesheets
				where !timesheet.Entries.Any() != false
				select timesheet).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task BooleanPropertyComparisonAsync()
		{
			var query = db.Timesheets.Where(t => t.Submitted == true);
			Assert.AreEqual(2, (await (query.ToListAsync())).Count);
		}

		[Test]
		public async Task BooleanPropertyAloneAsync()
		{
			var query = db.Timesheets.Where(t => t.Submitted);
			Assert.AreEqual(2, (await (query.ToListAsync())).Count);
		}
	}
}
#endif
