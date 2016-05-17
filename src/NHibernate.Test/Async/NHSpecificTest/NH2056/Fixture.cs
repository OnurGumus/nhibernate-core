#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2056
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUpdateInheritedClassAsync()
		{
			object savedId;
			using (var session = sessions.OpenSession())
				using (var t = session.BeginTransaction())
				{
					IDictionary address = new Dictionary<string, object>();
					address["BaseF1"] = "base1";
					address["BaseF2"] = "base2";
					address["AddressF1"] = "addressF1";
					address["AddressF2"] = "addressF2";
					savedId = session.Save("Address", address);
					await (t.CommitAsync());
				}

			using (var session = sessions.OpenSession())
				using (var t = session.BeginTransaction())
				{
					var query = session.CreateQuery("Update Address address set address.AddressF1 = :val1, address.AddressF2 = :val2 where ID=:theID");
					// The following works properly
					//IQuery query = session.CreateQuery("Update Address address set address.AddressF1 = :val1, address.BaseF1 = :val2 where ID=:theID");
					query.SetParameter("val1", "foo");
					query.SetParameter("val2", "bar");
					query.SetParameter("theID", savedId);
					query.ExecuteUpdate();
					await (t.CommitAsync());
				}

			using (var session = sessions.OpenSession())
				using (var t = session.BeginTransaction())
				{
					var updated = (IDictionary)session.Get("Address", savedId);
					Assert.That(updated["BaseF1"], Is.EqualTo("base1"));
					Assert.That(updated["BaseF2"], Is.EqualTo("base2"));
					Assert.That(updated["AddressF1"], Is.EqualTo("foo"));
					Assert.That(updated["AddressF2"], Is.EqualTo("bar"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
