#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1877
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{BirthDate = new DateTime(1988, 7, 21)}));
					await (session.SaveAsync(new Person{BirthDate = new DateTime(1987, 7, 22)}));
					await (session.SaveAsync(new Person{BirthDate = new DateTime(1986, 7, 23)}));
					await (session.SaveAsync(new Person{BirthDate = new DateTime(1987, 7, 24)}));
					await (session.SaveAsync(new Person{BirthDate = new DateTime(1988, 7, 25)}));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from Person").ExecuteUpdateAsync());
					await (tran.CommitAsync());
				}
		}

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
