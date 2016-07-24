#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1313
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1313";
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				Dialect.Dialect d = Dialect;
				ISQLFunction toReRegister = d.Functions["current_timestamp"];
				configuration.AddSqlFunction("MyCurrentTime", toReRegister);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task BugAsync()
		{
			A a = new A("NH1313");
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					DateTime result = await (s.CreateCriteria(typeof (A)).SetProjection(new SqlFunctionProjection("MyCurrentTime", NHibernateUtil.DateTime)).UniqueResultAsync<DateTime>());
					// we are simply checking that the function is parsed and executed
					await (s.DeleteAsync(a));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
