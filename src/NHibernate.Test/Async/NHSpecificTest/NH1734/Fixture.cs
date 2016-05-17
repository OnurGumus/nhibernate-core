#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1734
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ReturnsApropriateTypeWhenSumUsedWithSomeFormulaAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					double delta = 0.0000000000001;
					var query = session.CreateQuery("select sum(Amount*Price) from Product");
					var result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Price*Amount) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Price) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Amount) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (Int64)));
					Assert.That(result, Is.EqualTo(6));
				}
		}
	}
}
#endif
