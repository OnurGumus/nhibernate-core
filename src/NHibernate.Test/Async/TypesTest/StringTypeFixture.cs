#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StringTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task InsertNullValueAsync()
		{
			using (ISession s = OpenSession())
			{
				StringClass b = new StringClass();
				b.StringValue = null;
				await (s.SaveAsync(b));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				StringClass b = (StringClass)await (s.CreateCriteria(typeof (StringClass)).UniqueResultAsync());
				Assert.IsNull(b.StringValue);
				await (s.DeleteAsync(b));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
