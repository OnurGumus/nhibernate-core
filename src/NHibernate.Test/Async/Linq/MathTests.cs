#if NET_4_5
using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Dialect;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MathTestsAsync : LinqTestCaseAsync
	{
		private IQueryable<OrderLine> _orderLines;
		private void IgnoreIfNotSupported(string function)
		{
			if (!Dialect.Functions.ContainsKey(function))
				Assert.Ignore("Dialect {0} does not support '{1}' function", Dialect.GetType(), function);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			_orderLines = db.OrderLines.OrderBy(ol => ol.Id).Take(10).ToList().AsQueryable();
		}

		private void Test(Expression<Func<OrderLine, double>> selector)
		{
			var expected = _orderLines.Select(selector).ToList();
			var actual = db.OrderLines.OrderBy(ol => ol.Id).Select(selector).Take(10).ToList();
			Assert.AreEqual(expected.Count, actual.Count);
			for (var i = 0; i < expected.Count; i++)
				Assert.AreEqual(expected[i], actual[i], 0.000001);
		}
	}
}
#endif
