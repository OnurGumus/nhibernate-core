#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using NHibernate.Transaction;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.SessionIdLoggingContextTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PerfTest : BugTestCase
	{
		[Test]
		public async Task BenchmarkAsync()
		{
			using (var s = OpenSession())
			{
				var ticksAtStart = DateTime.Now.Ticks;
				var res = await (s.CreateCriteria<ClassA>().SetFetchMode("Children", FetchMode.Join).SetResultTransformer(Transformers.DistinctRootEntity).Add(Restrictions.Eq("Name", "Parent")).ListAsync<ClassA>());
				Console.WriteLine(TimeSpan.FromTicks(DateTime.Now.Ticks - ticksAtStart));
				Assert.AreEqual(noOfParents, res.Count);
				Assert.AreEqual(noOfChildrenForEachParent, res[0].Children.Count);
			}
		}
	}
}
#endif
