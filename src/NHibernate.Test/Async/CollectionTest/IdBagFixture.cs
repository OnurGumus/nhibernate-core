#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CollectionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdBagFixture : TestCase
	{
		[Test]
		public async Task SimpleAsync()
		{
			A a = new A();
			a.Name = "first generic type";
			a.Items = new List<string>();
			a.Items.Add("first string");
			a.Items.Add("second string");
			ISession s = OpenSession();
			await (s.SaveOrUpdateAsync(a));
			// this flush should test how NH wraps a generic collection with its
			// own persistent collection
			await (s.FlushAsync());
			s.Close();
			Assert.IsNotNull(a.Id);
			Assert.AreEqual("first string", (string)a.Items[0]);
			s = OpenSession();
			a = (A)await (s.LoadAsync(typeof (A), a.Id));
			Assert.AreEqual("first string", (string)a.Items[0], "first item should be 'first string'");
			Assert.AreEqual("second string", (string)a.Items[1], "second item should be 'second string'");
			// ensuring the correct generic type was constructed
			a.Items.Add("third string");
			Assert.AreEqual(3, a.Items.Count, "3 items in the list now");
			a.Items[1] = "new second string";
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
