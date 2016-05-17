#if NET_4_5
using System;
using NHibernate;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.PropertyRef
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class KeyPropertyRefFixture : TestCase
	{
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
