#if NET_4_5
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ReadOnly
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractReadOnlyTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task ConfigureAsync(Configuration configuration)
		{
			await (base.ConfigureAsync(configuration));
			configuration.SetProperty(Environment.GenerateStatistics, "true");
			configuration.SetProperty(Environment.BatchSize, "0");
		}

		protected override ISession OpenSession()
		{
			ISession session = base.OpenSession();
			session.CacheMode = CacheMode.Ignore;
			return session;
		}

		protected void ClearCounts()
		{
			Sfi.Statistics.Clear();
		}

		protected void AssertUpdateCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(count));
		}

		protected void AssertInsertCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(count));
		}

		protected void AssertDeleteCount(int count)
		{
			Assert.That(Sfi.Statistics.EntityDeleteCount, Is.EqualTo(count));
		}
	}
}
#endif
