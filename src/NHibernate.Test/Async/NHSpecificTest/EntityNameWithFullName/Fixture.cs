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
					await (s.SaveAsync("NHibernate.Test.NHSpecificTest.EntityNameWithFullName.Parent", new Dictionary<string, object>{{"SomeData", "hello"}}));
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var p = (IDictionary)(await (s.CreateQuery(@"select p from NHibernate.Test.NHSpecificTest.EntityNameWithFullName.Parent p where p.SomeData = :data").SetString("data", "hello").ListAsync()))[0];
					Assert.AreEqual("hello", p["SomeData"]);
				}
			}
		}

		[Test]
		public async Task OnlyOneSelectAsync()
		{
			using (var s = OpenSession())
			{
				var sf = s.SessionFactory;
				var onOffBefore = turnOnStatistics(s);
				try
				{
					using (s.BeginTransaction())
					{
						await (s.CreateQuery(@"select p from NHibernate.Test.NHSpecificTest.EntityNameWithFullName.Parent p where p.SomeData = :data").SetString("data", "hello").ListAsync());
					}

					Assert.AreEqual(1, sf.Statistics.QueryExecutionCount);
				}
				finally
				{
					sf.Statistics.IsStatisticsEnabled = onOffBefore;
				}
			}
		}
	}
}
#endif
