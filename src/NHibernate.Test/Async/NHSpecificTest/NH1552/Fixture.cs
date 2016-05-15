#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1552
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Paging_with_sql_works_as_expected_with_FirstResultAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					string sql = "select * from MyClass order by Name asc";
					IList<MyClass> list = await (session.CreateSQLQuery(sql).AddEntity(typeof (MyClass)).SetFirstResult(1).ListAsync<MyClass>());
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].Name, Is.EqualTo("sidar"));
					Assert.That(list[1].Name, Is.EqualTo("tuna"));
				}
			}
		}

		[Test]
		public async Task Paging_with_sql_works_as_expected_with_MaxResultAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					string sql = "select * from MyClass order by Name asc";
					IList<MyClass> list = await (session.CreateSQLQuery(sql).AddEntity(typeof (MyClass)).SetMaxResults(2).ListAsync<MyClass>());
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].Name, Is.EqualTo("berker"));
					Assert.That(list[1].Name, Is.EqualTo("sidar"));
				}
			}
		}

		[Test]
		public async Task Paging_with_sql_works_as_expected_with_FirstResultMaxResultAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					string sql = "select * from MyClass";
					IList<MyClass> list = await (session.CreateSQLQuery(sql).AddEntity(typeof (MyClass)).SetFirstResult(1).SetMaxResults(1).ListAsync<MyClass>());
					Assert.That(list.Count, Is.EqualTo(1));
					Assert.That(list[0].Name, Is.EqualTo("sidar"));
				}
			}
		}
	}
}
#endif
