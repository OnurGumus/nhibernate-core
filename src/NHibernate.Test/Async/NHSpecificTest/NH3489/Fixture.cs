#if NET_4_5
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3489
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task PerformanceTestAsync()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					IList<Order> orders = await (session.QueryOver<Order>().ListAsync());
					foreach (Order order in orders)
						order.Departments.ToList();
				}

			stopwatch.Stop();
			Console.WriteLine(stopwatch.Elapsed);
		}
	}
}
#endif
