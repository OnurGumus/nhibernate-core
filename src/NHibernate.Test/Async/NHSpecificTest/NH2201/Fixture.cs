#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2201
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseMutliCriteriaAndFetchSelectAsync()
		{
			using (var s = OpenSession())
			{
				Console.WriteLine("*** start");
				var results = await (s.CreateMultiCriteria().Add<Parent>(s.CreateCriteria<Parent>()).Add<Parent>(s.CreateCriteria<Parent>()).ListAsync());
				var result1 = (IList<Parent>)results[0];
				var result2 = (IList<Parent>)results[1];
				Assert.That(result1.Count, Is.EqualTo(2));
				Assert.That(result2.Count, Is.EqualTo(2));
				Console.WriteLine("*** end");
			}
		}
	}
}
#endif
