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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionFilterQueriesTestAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"One.hbm.xml", "Many.hbm.xml"};
			}
		}

		private One one;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			// create the objects to search on		
			one = new One();
			one.X = 20;
			one.Manies = new HashSet<Many>();
			Many many1 = new Many();
			many1.X = 10;
			many1.One = one;
			one.Manies.Add(many1);
			Many many2 = new Many();
			many2.X = 20;
			many2.One = one;
			one.Manies.Add(many2);
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(one));
					await (s.SaveAsync(many1));
					await (s.SaveAsync(many2));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Many"));
					await (session.DeleteAsync("from One"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task UpdateShouldBeDisallowedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					Assert.ThrowsAsync<QuerySyntaxException>(async () =>
					{
						await ((await (s.CreateFilterAsync(one2.Manies, "update Many set X = 1"))).ExecuteUpdateAsync());
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
					Assert.ThrowsAsync<QuerySyntaxException>(async () =>
					{
						await ((await (s.CreateFilterAsync(one2.Manies, "delete from Many"))).ExecuteUpdateAsync());
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
					Assert.ThrowsAsync<QuerySyntaxException>(async () =>
					{
						await ((await (s.CreateFilterAsync(one2.Manies, "insert into Many (X) select t0.X from Many t0"))).ExecuteUpdateAsync());
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
					await ((await (s.CreateFilterAsync(one2.Manies, "where this.X in (select t0.X from Many t0)"))).ListAsync());
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
					Assert.ThrowsAsync<QuerySyntaxException>(async () =>
					{
						await ((await (s.CreateFilterAsync(one2.Manies, "where this.X in (select X)"))).ListAsync());
					// Inner query for filter query should have FROM clause 
					}

					);
					t.Rollback();
				}
		}
	}
}
#endif
