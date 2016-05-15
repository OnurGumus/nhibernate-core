#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2955
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task EnumerableContainsAsync()
		{
			// ReSharper disable RedundantEnumerableCastCall
			var array = new[]{1, 3, 4}.OfType<int>();
			// ReSharper restore RedundantEnumerableCastCall
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var firstNames = await (session.CreateQuery("select e.FirstName from Employee e where e.Id in (:x)").SetParameterList("x", array).ListAsync<string>());
					Assert.AreEqual(3, firstNames.Count);
					Assert.AreEqual("Nancy", firstNames[0]);
					Assert.AreEqual("Janet", firstNames[1]);
					Assert.AreEqual("Margaret", firstNames[2]);
				}
		}

		[Test]
		public async Task GroupingContainsAsync()
		{
			var array = new[]{1, 3, 4}.ToLookup(x => 1).Single();
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var firstNames = await (session.CreateQuery("select e.FirstName from Employee e where e.Id in (:x)").SetParameterList("x", array).ListAsync<string>());
					Assert.AreEqual(3, firstNames.Count);
					Assert.AreEqual("Nancy", firstNames[0]);
					Assert.AreEqual("Janet", firstNames[1]);
					Assert.AreEqual("Margaret", firstNames[2]);
				}
		}
	}
}
#endif
