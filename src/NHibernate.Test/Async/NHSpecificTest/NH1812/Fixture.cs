#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1812
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AstBugBaseAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			var p = new Person();
			const string query = @"select p from Person p
                            left outer join p.PeriodCollection p1
                        where p1.Start > coalesce((select max(p2.Start) from Period p2), :nullStart)";
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

				await (s.CreateQuery(query).SetDateTime("nullStart", new DateTime(2001, 1, 1)).ListAsync<Person>());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AstBugAsync : AstBugBaseAsync
	{
	/* to the nh guy...
         * sorry for not coming up with a more realistic use case
         * We have a query that works fine with the old parser but not with the new AST parser
         * I've broke our complex query down to this... 
         * I believe the problem is when mixing aggregate methods with isnull()
         */
	}
}
#endif
