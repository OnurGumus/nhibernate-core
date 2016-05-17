#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ListIndex
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ListIndexFixture : TestCase
	{
		[Test]
		public async Task ListIndexBaseIsUsedAsync()
		{
			const int TheId = 2000;
			A a = new A();
			a.Name = "First";
			a.Id = TheId;
			B b = new B();
			b.AId = TheId;
			b.Name = "First B";
			a.Items.Add(b);
			B b2 = new B();
			b2.AId = TheId;
			b2.Name = "Second B";
			a.Items.Add(b2);
			ISession s = OpenSession();
			await (s.SaveAsync(a));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			A newA = await (s.GetAsync<A>(TheId));
			Assert.AreEqual(2, newA.Items.Count);
			int counter = 1;
			foreach (B item in newA.Items)
			{
				Assert.AreEqual(counter, item.ListIndex);
				counter++;
			}

			s.Close();
		}
	}
}
#endif
