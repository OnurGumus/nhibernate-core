#if NET_4_5
using System.Collections;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumCharTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task CanBeUsedAsDiscriminatorAsync()
		{
			EnumCharFoo foo = new EnumCharFoo();
			EnumCharBar bar = new EnumCharBar();
			foo.Id = 1;
			bar.Id = 2;
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(foo));
				await (s.SaveAsync(bar));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				s.Load<EnumCharFoo>(1);
				s.Load<EnumCharBar>(2);
				EnumCharBaz baz;
				baz = s.Load<EnumCharBaz>(1);
				Assert.AreEqual(SampleCharEnum.Dimmed, baz.Type);
				baz = s.Load<EnumCharBaz>(2);
				Assert.AreEqual(SampleCharEnum.Off, baz.Type);
			}
		}
	}
}
#endif
