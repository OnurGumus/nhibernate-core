#if NET_4_5
using System.Linq;
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Linq;
using NHibernate.Cfg;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2869
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task ConfigureAsync(Configuration configuration)
		{
			configuration.LinqToHqlGeneratorsRegistry<MyLinqToHqlGeneratorsRegistry>();
			await (base.ConfigureAsync(configuration));
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

		[Test]
		public void CustomExtensionWithConstantArgumentShouldBeIncludedInHqlProjection()
		{
			using (ISession session = this.OpenSession())
			{
				var projectionValue = (
					from c in session.Query<DomainClass>()where c.Name.IsOneInDbZeroInLocal("test") == 1
					select c.Name.IsOneInDbZeroInLocal("test")).FirstOrDefault();
				//If the value is 0, the call was done in .NET, if it's 1 it has been projected correctly
				Assert.AreEqual(1, projectionValue);
			}
		}
	}
}
#endif
