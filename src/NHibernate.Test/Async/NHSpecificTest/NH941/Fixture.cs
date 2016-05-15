#if NET_4_5
using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH941
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task WhenSaveOneThenShouldSaveManyAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var one = new MyClass();
					one.Relateds = new List<Related>{new Related(), new Related()};
					await (session.PersistAsync(one));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from Related").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from MyClass").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
