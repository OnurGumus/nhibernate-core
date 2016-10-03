#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH552
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH552";
			}
		}

		[Test]
		public async Task DeleteAndResaveAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Question q = new Question();
					q.Id = 1;
					await (session.SaveAsync(q));
					await (session.DeleteAsync(q));
					await (session.SaveAsync(q));
					Answer a = new Answer();
					a.Id = 1;
					a.Question = q;
					await (session.SaveAsync(a));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Answer"));
					await (session.DeleteAsync("from Question"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
