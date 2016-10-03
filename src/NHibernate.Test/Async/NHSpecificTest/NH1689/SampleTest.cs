#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1689
{
	using System.Collections.Generic;
	using Dialect;
	using NUnit.Framework;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				DomainClass entity = new DomainClass();
				entity.Id = 1;
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
				await (session.EvictAsync(entity));
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

		[Test]
		public async Task ShouldBeAbleToCallGenericMethodAsync()
		{
			using (ISession session = this.OpenSession())
			{
				DomainClass entity = await (session.LoadAsync<DomainClass>(1));
				IList<string> inputStrings = entity.GetListOfTargetType<string>("arg");
				Assert.That(inputStrings.Count == 0);
			}
		}
	}
}
#endif
