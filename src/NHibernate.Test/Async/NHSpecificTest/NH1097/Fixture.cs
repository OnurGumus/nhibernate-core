#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1097
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Name = "Fabio"}));
					await (session.SaveAsync(new Person{Name = "Dario"}));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Name = "Fabio"}));
					await (session.SaveAsync(new Person{Name = "Dario"}));
					await (session.DeleteAsync("from Person"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task ThrowsExceptionWhenColumnNameIsUsedInQueryAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					Assert.ThrowsAsync<QueryException>(async delegate
					{
						var query = session.CreateQuery("from Person p where p.namecolumn=:nameOfPerson");
						query.SetString("nameOfPerson", "Dario");
						await (query.ListAsync());
					}

					);
				}
		}
	}
}
#endif
