#if NET_4_5
using System;
using System.Diagnostics;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3037
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
		[TestCase(10)]
		[TestCase(100)]
		[TestCase(1000)]
		[TestCase(10000)]
		[TestCase(20000)]
		[TestCase(30000)]
		[TestCase(40000)]
		public async Task SortInsertionActionsAsync(int iterations)
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					for (int i = 1; i <= iterations; i++)
					{
						await (session.SaveAsync(new Entity()
						{Id = i, Name = i.ToString()}));
					}

					var impl = ((NHibernate.Impl.SessionImpl)session);
					var stopwatch = Stopwatch.StartNew();
					impl.ActionQueue.SortActions();
					stopwatch.Stop();
					System.Console.WriteLine(stopwatch.Elapsed);
				}
		}
	}
}
#endif
