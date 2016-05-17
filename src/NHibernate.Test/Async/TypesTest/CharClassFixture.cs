#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CharClassFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			var basic = new CharClass{Id = 1, NormalChar = 'A'};
			using (var s = OpenSession())
			{
				await (s.SaveAsync(basic));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				CharClass saved = null;
				Assert.That(async () => saved = await (s.GetAsync<CharClass>(1)), Throws.Nothing);
				Assert.That(saved.NormalChar, Is.EqualTo('A'));
				Assert.That(saved.NullableChar, Is.Null);
				await (s.DeleteAsync(saved));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
