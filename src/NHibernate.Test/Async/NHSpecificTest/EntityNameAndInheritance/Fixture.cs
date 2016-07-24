#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.EntityNameAndInheritance
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private int id;
		private const string entityName = "SuperClass";
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					id = (int)await (s.SaveAsync(entityName, new Hashtable()));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task DoesNotCrashAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.IsNotNull(await (s.GetAsync(entityName, id)));
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateSQLQuery("delete from " + entityName).ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
