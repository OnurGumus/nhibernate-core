﻿#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using NHibernate.Dialect;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3377
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob", Age = "17", Solde = "5.4"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally", Age = "16"};
					await (session.SaveAsync(e2));
					var e3 = new Entity{Name = "true", Age = "10"};
					await (session.SaveAsync(e3));
					var e4 = new Entity{Name = "2014-10-13", Age = "11"};
					await (session.SaveAsync(e4));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldBeAbleToCallConvertToInt32FromStringParameterAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from e in session.Query<Entity>()where e.Name == "Bob"
						select Convert.ToInt32(e.Age)).ToListAsync());
					Assert.That(result, Has.Count.EqualTo(1));
					Assert.That(result[0], Is.EqualTo(17));
				}
		}

		[Test]
		public async Task ShouldBeAbleToCallConvertToInt32FromStringParameterInMaxAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.Query<Entity>().MaxAsync(e => Convert.ToInt32(e.Age)));
					Assert.That(result, Is.EqualTo(17));
				}
		}

		[Test]
		public async Task ShouldBeAbleToCallConvertToBooleanFromStringParameterAsync()
		{
			if (Dialect is SQLiteDialect || Dialect is FirebirdDialect || Dialect is MySQLDialect || Dialect is Oracle8iDialect)
				Assert.Ignore(Dialect.GetType() + " is not supported");
			//NH-3720
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.Query<Entity>().Where(x => x.Name == "true").Select(x => Convert.ToBoolean(x.Name)).SingleAsync());
					Assert.That(result, Is.True);
				}
		}

		[Test]
		public async Task ShouldBeAbleToCallConvertToDateTimeFromStringParameterAsync()
		{
			if (Dialect is Oracle8iDialect)
				Assert.Ignore(Dialect.GetType() + " is not supported");
			//NH-3720
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.Query<Entity>().Where(x => x.Name == "2014-10-13").Select(x => Convert.ToDateTime(x.Name)).SingleAsync());
					Assert.That(result, Is.EqualTo(new DateTime(2014, 10, 13)));
				}
		}
	}
}
#endif
