#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using System.Text;
using NHibernate.Cfg;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2055
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return (dialect is Dialect.MsSql2000Dialect);
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			cfg = configuration;
		}

		[Test]
		public async Task CanCreateAndDropSchemaAsync()
		{
			using (var s = sessions.OpenSession())
			{
				using (var cmd = s.Connection.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = "test_proc1";
					Assert.AreEqual(1, await (cmd.ExecuteScalarAsync()));
					cmd.CommandText = "test_proc2";
					Assert.AreEqual(2, await (cmd.ExecuteScalarAsync()));
				}
			}
		}
	}
}
#endif
