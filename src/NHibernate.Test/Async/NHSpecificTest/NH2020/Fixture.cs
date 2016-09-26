#if NET_4_5
using NUnit.Framework;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using NHibernate.Test.ExceptionsTest;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2020
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Cfg.Configuration configuration)
		{
			configuration.SetProperty(Cfg.Environment.BatchSize, "10");
			configuration.SetProperty(Cfg.Environment.SqlExceptionConverter, typeof (MSSQLExceptionConverterExample).AssemblyQualifiedName);
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from One"));
					await (s.DeleteAsync("from Many"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task ISQLExceptionConverter_gets_called_if_batch_size_enabledAsync()
		{
			long oneId;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var one = new One();
					await (s.SaveAsync(one));
					var many = new Many{One = one};
					await (s.SaveAsync(many));
					await (tx.CommitAsync());
					oneId = one.Id;
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var one = await (s.LoadAsync<One>(oneId));
					await (s.DeleteAsync(one));
					Assert.That(async () => await (tx.CommitAsync()), Throws.TypeOf<ConstraintViolationException>());
				}
		}
	}
}
#endif
