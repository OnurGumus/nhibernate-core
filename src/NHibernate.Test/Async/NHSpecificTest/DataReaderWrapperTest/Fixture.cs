#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.DataReaderWrapperTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private const int id = 1333;
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnSetUpAsync()
		{
			var ent = new TheEntity{TheValue = "Hola", Id = id};
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(ent));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseDatareadersGetValueAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var crit = s.CreateCriteria(typeof (TheEntity));
					var multi = s.CreateMultiCriteria();
					multi.Add(crit);
					var res = (IList)(await (multi.ListAsync()))[0];
					Assert.That(res.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
