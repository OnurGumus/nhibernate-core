#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StringClobTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			ISession s = OpenSession();
			StringClobClass b = new StringClobClass();
			b.StringClob = "foo/bar/baz";
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			b = (StringClobClass)await (s.LoadAsync(typeof (StringClobClass), b.Id));
			Assert.AreEqual("foo/bar/baz", b.StringClob);
			await (s.DeleteAsync(b));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task LongStringAsync()
		{
			string longString = new string ('x', 10000);
			using (ISession s = OpenSession())
			{
				StringClobClass b = new StringClobClass();
				b.StringClob = longString;
				await (s.SaveAsync(b));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				StringClobClass b = (StringClobClass)await (s.CreateCriteria(typeof (StringClobClass)).UniqueResultAsync());
				Assert.AreEqual(longString, b.StringClob);
				await (s.DeleteAsync(b));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task InsertNullValueAsync()
		{
			using (ISession s = OpenSession())
			{
				StringClobClass b = new StringClobClass();
				b.StringClob = null;
				await (s.SaveAsync(b));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				StringClobClass b = (StringClobClass)await (s.CreateCriteria(typeof (StringClobClass)).UniqueResultAsync());
				Assert.IsNull(b.StringClob);
				await (s.DeleteAsync(b));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
