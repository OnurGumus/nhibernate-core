#if NET_4_5
using System;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH864
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task OptimisticLockingAsync()
		{
			using (ISession s = OpenSession())
			{
				ObjectWithNullableInt32 obj = new ObjectWithNullableInt32();
				await (s.SaveAsync(obj));
				await (s.FlushAsync());
				obj.NullInt32 = 1;
				await (s.FlushAsync());
				obj.NullInt32 = NullableInt32.Default;
				await (s.FlushAsync());
				await (s.DeleteAsync(obj));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
