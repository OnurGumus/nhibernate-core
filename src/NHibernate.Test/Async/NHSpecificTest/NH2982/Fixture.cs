#if NET_4_5
using System;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2982
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Id = 1, Name = "A"};
					await (session.SaveAsync(e1));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task SimpleExpressionWithProxyAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = await (session.LoadAsync<Entity>(1));
					var restriction = Restrictions.Eq("A", a);
					Assert.AreEqual("A = Entity#1", restriction.ToString());
				}
		}
	}
}
#endif
