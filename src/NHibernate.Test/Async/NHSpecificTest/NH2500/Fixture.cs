#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2500
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task TestLinqProjectionExpressionDoesntCacheParametersAsync()
		{
			using (ISession session = Sfi.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					this.count = 1;
					var foos1 = session.Query<Foo>().Where(x => x.Name == "Banana").Select(x => new
					{
					x.Name, count, User = "abc"
					}

					).First();
					this.count = 2;
					var foos2 = session.Query<Foo>().Where(x => x.Name == "Egg").Select(x => new
					{
					x.Name, count, User = "def"
					}

					).First();
					Assert.AreEqual(1, foos1.count);
					Assert.AreEqual(2, foos2.count);
					Assert.AreEqual("abc", foos1.User);
					Assert.AreEqual("def", foos2.User);
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
