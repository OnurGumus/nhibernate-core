#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2044
{
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
				entity.Symbol = 'S';
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				string hql = "from DomainClass";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task IgnoreCaseShouldWorkWithCharCorrectlyAsync()
		{
			using (ISession session = this.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (DomainClass), "domain");
				criteria.Add(NHibernate.Criterion.Expression.Eq("Symbol", 's').IgnoreCase());
				IList<DomainClass> list = await (criteria.ListAsync<DomainClass>());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
