#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1877
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanGroupByWithPropertyNameAsync()
		{
			using (var session = OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Person)).SetProjection(Projections.GroupProperty("BirthDate"), Projections.Count("Id"));
				var result = await (crit.ListAsync());
				Assert.That(result, Has.Count.EqualTo(5));
			}
		}

		[Test]
		public async Task CanGroupByWithSqlFunctionProjectionAsync()
		{
			using (var session = OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Person)).SetProjection(Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("BirthDate"))));
				var result = await (crit.UniqueResultAsync());
				Assert.That(result, Is.EqualTo(7));
			}
		}
	}
}
#endif
