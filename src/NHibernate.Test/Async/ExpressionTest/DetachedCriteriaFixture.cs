#if NET_4_5
using System.Collections;
using NHibernate.DomainModel;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DetachedCriteriaFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Componentizable.hbm.xml"};
			}
		}

		[Test]
		public async Task CanUseDetachedCriteriaToQueryAsync()
		{
			using (ISession s = OpenSession())
			{
				Componentizable master = new Componentizable();
				master.NickName = "master";
				await (s.SaveAsync(master));
				await (s.FlushAsync());
			}

			DetachedCriteria detachedCriteria = DetachedCriteria.For(typeof (Componentizable));
			detachedCriteria.Add(Expression.Eq("NickName", "master"));
			using (ISession s = OpenSession())
			{
				Componentizable componentizable = (Componentizable)await (detachedCriteria.GetExecutableCriteria(s).UniqueResultAsync());
				Assert.AreEqual("master", componentizable.NickName);
				await (s.DeleteAsync(componentizable));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
