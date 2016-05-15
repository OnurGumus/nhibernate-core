#if NET_4_5
using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MiscellaneousTextFixture : LinqTestCase
	{
		[Test]
		public async Task SelectFromObjectAsync()
		{
			using (var s = OpenSession())
			{
				var hql = await (s.CreateQuery("from System.Object o").ListAsync());
				var r =
					from o in s.Query<object>()select o;
				var l = r.ToList();
				Assert.AreEqual(hql.Count, l.Count);
			}
		}
	}
}
#endif
