#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Collections;
using NHibernate.DomainModel;
using System.Threading.Tasks;

namespace NHibernate.Test.CollectionFilterTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionFilterQueriesTest : TestCase
	{
		[Test]
		public async Task UpdateShouldBeDisallowedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					Assert.Throws<QuerySyntaxException>(() =>
					{
						s.CreateFilter(one2.Manies, "update Many set X = 1").ExecuteUpdate();
					// Collection filtering disallows DML queries
					}

					);
					t.Rollback();
				}
		}

		[Test]
		public async Task DeleteShouldBeDisallowedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					Assert.Throws<QuerySyntaxException>(() =>
					{
						s.CreateFilter(one2.Manies, "delete from Many").ExecuteUpdate();
					// Collection filtering disallows DML queries
					}

					);
					t.Rollback();
				}
		}

		[Test]
		public async Task InsertShouldBeDisallowedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					Assert.Throws<QuerySyntaxException>(() =>
					{
						s.CreateFilter(one2.Manies, "insert into Many (X) select t0.X from Many t0").ExecuteUpdate();
					// Collection filtering disallows DML queries
					}

					);
					t.Rollback();
				}
		}

		[Test]
		public async Task InnerSubqueryShouldNotBeFilteredAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					s.CreateFilter(one2.Manies, "where this.X in (select t0.X from Many t0)").List();
					// Filter should only affect outer query, not inner
					t.Rollback();
				}
		}

		[Test]
		public async Task InnerSubqueryMustHaveFromClauseAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					Assert.Throws<QuerySyntaxException>(() =>
					{
						s.CreateFilter(one2.Manies, "where this.X in (select X)").List();
					// Inner query for filter query should have FROM clause 
					}

					);
					t.Rollback();
				}
		}
	}
}
#endif
