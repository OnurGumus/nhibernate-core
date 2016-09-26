#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryReuseTestsAsync : LinqTestCaseAsync
	{
		private IQueryable<User> _query;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			_query = db.Users;
		}

		private void AssertQueryReuseable()
		{
			IList<User> users = _query.ToList();
			Assert.AreEqual(3, users.Count);
		}
	}
}
#endif
