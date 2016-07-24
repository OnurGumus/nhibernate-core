#if NET_4_5
using System.Collections;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1836
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Entity{Id = 1}));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task AliasToBeanTransformerShouldApplyCorrectlyToMultiQueryAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					IMultiQuery multiQuery = s.CreateMultiQuery().Add(s.CreateQuery("select entity.Id as EntityId from Entity entity").SetResultTransformer(Transformers.AliasToBean(typeof (EntityDTO))));
					IList results = null;
					Assert.That(async () => results = await (multiQuery.ListAsync()), Throws.Nothing);
					var elementOfFirstResult = ((IList)results[0])[0];
					Assert.That(elementOfFirstResult, Is.TypeOf<EntityDTO>().And.Property("EntityId").EqualTo(1));
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from System.Object").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
