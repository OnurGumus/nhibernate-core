#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.EntityNameWithFullName
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanPersistAndReadAsync()
		{
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					s.Save("NHibernate.Test.NHSpecificTest.EntityNameWithFullName.Parent", new Dictionary<string, object>{{"SomeData", "hello"}});
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var p = (IDictionary)s.CreateQuery(@"select p from NHibernate.Test.NHSpecificTest.EntityNameWithFullName.Parent p where p.SomeData = :data").SetString("data", "hello").List()[0];
					Assert.AreEqual("hello", p["SomeData"]);
				}
			}
		}
	}
}
#endif
