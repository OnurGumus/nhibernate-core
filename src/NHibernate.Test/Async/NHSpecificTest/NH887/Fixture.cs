#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH887
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task PropertyRefReferencingParentPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				Child child = new Child();
				child.UniqueKey = 10;
				await (s.SaveAsync(child));
				Consumer consumer = new Consumer();
				consumer.Child = child;
				await (s.SaveAsync(consumer));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Consumer"));
				await (s.DeleteAsync("from Child"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
