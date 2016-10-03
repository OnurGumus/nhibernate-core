#if NET_4_5
using System.Collections;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumCharTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "EnumChar";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			EnumCharClass basic = new EnumCharClass();
			basic.Id = 1;
			basic.EnumValue = SampleCharEnum.Dimmed;
			EnumCharClass basic2 = new EnumCharClass();
			basic2.Id = 2;
			basic2.EnumValue = SampleCharEnum.On;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.SaveAsync(basic2));
			await (s.FlushAsync());
			s.Close();
		}

		protected override async Task OnTearDownAsync()
		{
			ISession s = OpenSession();
			await (s.DeleteAsync("from EnumCharClass"));
			await (s.DeleteAsync("from EnumCharBaz"));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ReadFromLoadAsync()
		{
			using (ISession s = OpenSession())
			{
				EnumCharClass basic = (EnumCharClass)await (s.LoadAsync(typeof (EnumCharClass), 1));
				Assert.AreEqual(SampleCharEnum.Dimmed, basic.EnumValue);
				EnumCharClass basic2 = (EnumCharClass)await (s.LoadAsync(typeof (EnumCharClass), 2));
				Assert.AreEqual(SampleCharEnum.On, basic2.EnumValue);
			}
		}

		[Test]
		public async Task ReadFromQueryUsingValueAsync()
		{
			using (ISession s = OpenSession())
			{
				IList results;
				IQuery q = s.CreateQuery("from EnumCharClass as ecc where ecc.EnumValue=:value");
				q.SetParameter("value", SampleCharEnum.On, new EnumCharType<SampleCharEnum>());
				results = await (q.ListAsync());
				Assert.AreEqual(1, results.Count, "only 1 was 'On'");
				q.SetParameter("value", SampleCharEnum.Off, new EnumCharType<SampleCharEnum>());
				results = await (q.ListAsync());
				Assert.AreEqual(0, results.Count, "should not be any in the 'Off' status");
			}
		}

		[Test]
		public async Task ReadFromQueryUsingStringAsync()
		{
			using (ISession s = OpenSession())
			{
				IList results;
				IQuery q = s.CreateQuery("from EnumCharClass as ecc where ecc.EnumValue=:value");
				q.SetString("value", "N");
				results = await (q.ListAsync());
				Assert.AreEqual(1, results.Count, "only 1 was \"N\" string");
				q.SetString("value", "F");
				results = await (q.ListAsync());
				Assert.AreEqual(0, results.Count, "should not be any in the \"F\" string");
			}
		}

		[Test]
		public async Task ReadFromQueryUsingCharAsync()
		{
			using (ISession s = OpenSession())
			{
				IList results;
				IQuery q = s.CreateQuery("from EnumCharClass as ecc where ecc.EnumValue=:value");
				q.SetCharacter("value", 'N');
				results = await (q.ListAsync());
				Assert.AreEqual(1, results.Count, "only 1 was 'N' char");
				q.SetCharacter("value", 'F');
				results = await (q.ListAsync());
				Assert.AreEqual(0, results.Count, "should not be any in the 'F' char");
			}
		}

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
				await (s.LoadAsync<EnumCharFoo>(1));
				await (s.LoadAsync<EnumCharBar>(2));
				EnumCharBaz baz;
				baz = await (s.LoadAsync<EnumCharBaz>(1));
				Assert.AreEqual(SampleCharEnum.Dimmed, baz.Type);
				baz = await (s.LoadAsync<EnumCharBaz>(2));
				Assert.AreEqual(SampleCharEnum.Off, baz.Type);
			}
		}
	}
}
#endif
