#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Subclass.EnumDiscriminator
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumDiscriminatorFixture : TestCase
	{
		[Test]
		public async Task PersistsDefaultDiscriminatorValueAsync()
		{
			Foo foo = new Foo();
			foo.Id = 1;
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(foo));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Baz baz = s.Load<Baz>(1L);
				Assert.AreEqual(Colors.Green, baz.Color);
			}
		}

		[Test]
		public async Task CanConvertOneTypeToAnotherAsync()
		{
			Foo foo = new Foo();
			foo.Id = 1;
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(foo));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Baz baz = s.Load<Baz>(1L);
				baz.Color = Colors.Blue;
				await (s.SaveAsync(baz));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Bar bar = s.Load<Bar>(1L);
			}
		}
	}
}
#endif
