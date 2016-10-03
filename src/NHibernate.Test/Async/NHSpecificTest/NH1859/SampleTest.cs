#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1859
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(new DomainClass{Id = 1}));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from DomainClass"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task NativeQueryWithTwoCommentsAsync()
		{
			using (ISession session = OpenSession())
			{
				IQuery qry = session.CreateSQLQuery("select /* first comment */ o.* /* second comment*/ from domainclass o").AddEntity("o", typeof (DomainClass));
				var res = await (qry.ListAsync<DomainClass>());
				Assert.AreEqual(res[0].Id, 1);
			}
		}
	}
}
#endif
