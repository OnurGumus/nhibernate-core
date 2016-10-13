#if NET_4_5
using System.Linq;
using System.Threading;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NUnit.Framework;
#if NET_4_5
using System.Threading.Tasks;
#endif
using Environment = NHibernate.Cfg.Environment;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FallbackFixtureAsync : FutureFixtureAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			var cp = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties);
			return !cp.Driver.SupportsMultipleQueries;
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			if (Dialect is MsSql2000Dialect)
			{
				configuration.Properties[Environment.ConnectionDriver] = typeof (TestDriverThatDoesntSupportQueryBatching).AssemblyQualifiedName;
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenSession())
			{
				await (session.DeleteAsync("from Person"));
				await (session.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		private async Task<int> CreatePersonAsync()
		{
			using (var session = sessions.OpenSession())
			{
				var person = new Person();
				await (session.SaveAsync(person));
				await (session.FlushAsync());
				return person.Id;
			}
		}
	}
}
#endif
