#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1349
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
