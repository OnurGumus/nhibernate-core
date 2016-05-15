#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1963
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CacheableQueryOnByteArray : BugTestCase
	{
		[Test]
		public async Task Should_be_able_to_do_cacheable_query_on_byte_array_fieldAsync()
		{
			using (ISession session = this.OpenSession())
			{
				var data = new byte[]{1, 2, 3};
				var result = await (session.CreateQuery("from DomainClass d where d.ByteData = :data").SetBinary("data", data).SetCacheable(true).UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(result);
			}

			using (ISession session = this.OpenSession())
			{
				var data = new byte[]{1, 2, 3};
				var result = await (session.CreateQuery("from DomainClass d where d.ByteData = :data").SetBinary("data", data).SetCacheable(true).UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(result);
			}
		}

		[Test]
		public async Task Should_work_when_query_is_not_cachableAsync()
		{
			using (ISession session = this.OpenSession())
			{
				var data = new byte[]{1, 2, 3};
				var result = await (session.CreateQuery("from DomainClass d where d.ByteData = :data").SetParameter("data", data).UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(result);
			}

			using (ISession session = this.OpenSession())
			{
				var data = new byte[]{1, 2, 3};
				var result = await (session.CreateQuery("from DomainClass d where d.ByteData = :data").SetParameter("data", data).UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(result);
			}
		}
	}
}
#endif
