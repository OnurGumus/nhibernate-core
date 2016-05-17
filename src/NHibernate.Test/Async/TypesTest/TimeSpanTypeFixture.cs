#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanTypeFixture2 : TypeFixtureBase
	{
		[Test]
		public async Task SavingAndRetrievingAsync()
		{
			var ticks = new TimeSpan(1982);
			var entity = new TimeSpanClass{TimeSpanValue = ticks};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			TimeSpanClass entityReturned;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					entityReturned = s.CreateQuery("from TimeSpanClass").UniqueResult<TimeSpanClass>();
					Assert.AreEqual(ticks, entityReturned.TimeSpanValue);
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(entityReturned));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
