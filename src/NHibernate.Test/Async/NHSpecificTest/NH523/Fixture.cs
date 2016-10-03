#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH523
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH523";
			}
		}

		[Test]
		public async Task MergeLazyAsync()
		{
			ClassA a = new ClassA();
			a.B = new ClassB();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.Merge(a);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync(a));
					await (s.DeleteAsync(a.B));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
