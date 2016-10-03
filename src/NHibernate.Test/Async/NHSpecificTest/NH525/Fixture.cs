#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH525
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH525";
			}
		}

		[Test]
		public async Task DoSomethingAsync()
		{
			NonAbstract obj = new NonAbstract();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(obj));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					AbstractBase baseObj = (AbstractBase)await (s.LoadAsync(typeof (AbstractBase), obj.Id));
					Assert.AreEqual(NonAbstract.AbstractMethodResult, baseObj.AbstractMethod());
					await (s.DeleteAsync(baseObj));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
