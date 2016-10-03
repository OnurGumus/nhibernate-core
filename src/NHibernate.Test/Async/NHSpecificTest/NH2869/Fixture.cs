#if NET_4_5
using System.Linq;
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Linq;
using NHibernate.Cfg;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2869
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.LinqToHqlGeneratorsRegistry<MyLinqToHqlGeneratorsRegistry>();
			base.Configure(configuration);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				var entity = new DomainClass();
				entity.Id = 1;
				entity.Name = "Test";
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				string hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect as MsSql2008Dialect != null;
		}
	}
}
#endif
