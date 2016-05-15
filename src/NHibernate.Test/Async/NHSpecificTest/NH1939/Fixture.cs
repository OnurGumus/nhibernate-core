#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using System.Text;
using NHibernate.Cfg;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1939
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Can_Parameterise_Auxiliary_Database_ObjectsAsync()
		{
			schemaBuilder = new StringBuilder();
			SchemaExport schemaExport = new SchemaExport(cfg);
			await (schemaExport.ExecuteAsync(AddString, false, false));
			string schema = schemaBuilder.ToString();
			Assert.That(schema.Contains("select 'drop script'"), Is.True, "schema drop script not exported");
			Assert.That(schema.Contains("select 'create script'"), Is.True, "parameterised schema create script not exported");
		}
	}
}
#endif
