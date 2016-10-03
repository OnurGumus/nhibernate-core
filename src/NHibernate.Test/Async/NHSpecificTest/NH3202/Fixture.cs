#if NET_4_5
using System.Data.Common;
using System.Text.RegularExpressions;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3202
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			if (!(Dialect is MsSql2008Dialect))
				Assert.Ignore("Test is for MS SQL Server dialect only (custom dialect).");
			if (!Environment.ConnectionDriver.Contains("SqlClientDriver"))
				Assert.Ignore("Test is for MS SQL Server driver only (custom driver is used).");
			cfg.SetProperty(Environment.Dialect, typeof (OffsetStartsAtOneTestDialect).AssemblyQualifiedName);
			cfg.SetProperty(Environment.ConnectionDriver, typeof (OffsetTestDriver).AssemblyQualifiedName);
		}

		private OffsetStartsAtOneTestDialect OffsetStartsAtOneTestDialect
		{
			get
			{
				return (OffsetStartsAtOneTestDialect)Sfi.Dialect;
			}
		}

		private OffsetTestDriver CustomDriver
		{
			get
			{
				return (OffsetTestDriver)Sfi.ConnectionProvider.Driver;
			}
		}

		protected override async Task OnSetUpAsync()
		{
			CustomDriver.OffsetStartsAtOneTestDialect = OffsetStartsAtOneTestDialect;
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new SequencedItem(1)));
					await (session.SaveAsync(new SequencedItem(2)));
					await (session.SaveAsync(new SequencedItem(3)));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from SequencedItem"));
					await (t.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task OffsetNotStartingAtOneSetsParameterToSkipValueAsync()
		{
			OffsetStartsAtOneTestDialect.ForceOffsetStartsAtOne = false;
			using (var session = OpenSession())
			{
				var item2 = await (session.QueryOver<SequencedItem>().OrderBy(i => i.I).Asc.Take(1).Skip(2).SingleOrDefaultAsync<SequencedItem>());
				Assert.That(CustomDriver.OffsetParameterValueFromCommand, Is.EqualTo(2));
			}
		}

		[Test]
		public async Task OffsetStartingAtOneSetsParameterToSkipValuePlusOneAsync()
		{
			OffsetStartsAtOneTestDialect.ForceOffsetStartsAtOne = true;
			using (var session = OpenSession())
			{
				var item2 = await (session.QueryOver<SequencedItem>().OrderBy(i => i.I).Asc.Take(1).Skip(2).SingleOrDefaultAsync<SequencedItem>());
				Assert.That(CustomDriver.OffsetParameterValueFromCommand, Is.EqualTo(3));
			}
		}
	}
}
#endif
