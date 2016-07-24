#if NET_4_5
using System;
using NHibernate;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyRef
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class KeyPropertyRefFixtureAsync : TestCaseAsync
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[]{"PropertyRef.KeyPropertyRef.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from B"));
				await (s.FlushAsync());
				await (s.DeleteAsync("from A"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task PropertyRefUsesOtherColumnAsync()
		{
			const int ExtraId = 500;
			A a = new A();
			a.Name = "First";
			a.ExtraId = ExtraId;
			B b = new B();
			b.Id = ExtraId;
			b.Name = "Second";
			ISession s = OpenSession();
			await (s.SaveAsync(a));
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			A newA = await (s.GetAsync<A>(a.Id));
			Assert.AreEqual(1, newA.Items.Count);
			s.Close();
		}
	}
}
#endif
