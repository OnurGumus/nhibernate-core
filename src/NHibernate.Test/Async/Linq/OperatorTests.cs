﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.Linq
{
    using System.Threading.Tasks;
    using System.Threading;
    [TestFixture]
    public class OperatorTestsAsync : LinqTestCase
    {
        [Test]
        public async Task ModAsync()
        {
            Assert.AreEqual(2, await (session.Query<TimesheetEntry>().Where(a => a.NumberOfHours % 7 == 0).CountAsync(CancellationToken.None)));
        }

		[Test]
		public async Task UnaryMinusAsync()
		{
			Assert.AreEqual(1, await (session.Query<TimesheetEntry>().CountAsync(a => -a.NumberOfHours == -7, CancellationToken.None)));
		}

		[Test]
		public async Task UnaryPlusAsync()
		{
			// Ensure expression tree contains UnaryPlus
			var param = Expression.Parameter(typeof(TimesheetEntry), "e");
			var expr = Expression.Equal(Expression.UnaryPlus(Expression.PropertyOrField(param, "NumberOfHours")), Expression.Constant(7));
			var predicate = Expression.Lambda<Func<TimesheetEntry, bool>>(expr, param);
			Assert.AreEqual(1, await (session.Query<TimesheetEntry>().CountAsync(predicate, CancellationToken.None)));
		}
	}
}
