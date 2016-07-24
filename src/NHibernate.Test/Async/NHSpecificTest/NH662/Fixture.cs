#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH662
{
	[TestFixture, Ignore("Not supported.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task UseDerivedClassAsync()
		{
			object savedId;
			var d = new Derived{Description = "something"};
			using (ISession s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					savedId = await (s.SaveAsync(d));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					d = await (s.LoadAsync<Derived>(savedId));
					Assert.That(d.Description, Is.EqualTo("something"));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(d));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
