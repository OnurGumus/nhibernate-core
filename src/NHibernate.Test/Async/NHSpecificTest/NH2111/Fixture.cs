#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2111
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SyncRootOnLazyLoadAsync()
		{
			A a = new A();
			a.Name = "first generic type";
			a.LazyItems = new List<string>();
			a.LazyItems.Add("first string");
			a.LazyItems.Add("second string");
			a.LazyItems.Add("third string");
			ISession s = OpenSession();
			await (s.SaveOrUpdateAsync(a));
			await (s.FlushAsync());
			s.Close();
			Assert.IsNotNull(((ICollection)a.LazyItems).SyncRoot);
			Assert.AreEqual("first string", a.LazyItems[0]);
			s = OpenSession();
			a = (A)await (s.LoadAsync(typeof (A), a.Id));
			Assert.IsNotNull(((ICollection)a.LazyItems).SyncRoot);
			Assert.AreEqual("first string", a.LazyItems[0]);
			s.Close();
		}
	}
}
#endif
