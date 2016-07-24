#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1895
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task SaveTestAsync()
		{
			var o = new Order{Id = Guid.NewGuid(), Name = "Test Order"};
			for (int i = 0; i < 5; i++)
			{
				var d = new Detail{Id = Guid.NewGuid(), Name = "Test Detail " + i, Parent = o};
				o.Details.Add(d);
			}

			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(o));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync(o));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
