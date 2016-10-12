#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OperatorTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task ModAsync()
		{
			Assert.AreEqual(2, await (session.Query<TimesheetEntry>().Where(a => a.NumberOfHours % 7 == 0).CountAsync()));
		}
	}
}
#endif
