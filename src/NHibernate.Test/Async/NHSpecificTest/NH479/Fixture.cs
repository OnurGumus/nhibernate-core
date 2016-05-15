#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH479
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task MergeTestAsync()
		{
			Main main = new Main();
			Aggregate aggregate = new Aggregate();
			main.Aggregate = aggregate;
			aggregate.Main = main;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(main));
					await (s.SaveAsync(aggregate));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.MergeAsync(main));
					await (s.MergeAsync(aggregate));
					await (t.CommitAsync());
				}

				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Aggregate"));
					await (s.DeleteAsync("from Main"));
					await (t.CommitAsync());
				}
			}
		}
	}
}
#endif
