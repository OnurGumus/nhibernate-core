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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					MyClass newServ = new MyClass();
					newServ.Name = "tuna";
					MyClass newServ2 = new MyClass();
					newServ2.Name = "sidar";
					MyClass newServ3 = new MyClass();
					newServ3.Name = "berker";
					await (session.SaveAsync(newServ));
					await (session.SaveAsync(newServ2));
					await (session.SaveAsync(newServ3));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from MyClass"));
					await (tran.CommitAsync());
				}
			}
		}

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
