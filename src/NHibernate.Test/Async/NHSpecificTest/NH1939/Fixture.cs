﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Tool.hbm2ddl;
using System.Text;
using NHibernate.Cfg;

namespace NHibernate.Test.NHSpecificTest.NH1939
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{

		private StringBuilder schemaBuilder;

		private void AddString(string sqlString)
		{
			schemaBuilder.Append(sqlString);
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return (dialect is Dialect.MsSql2000Dialect);
		}


		[Test] 
		public async Task Can_Parameterise_Auxiliary_Database_ObjectsAsync() 
		{
			schemaBuilder = new StringBuilder();

			SchemaExport schemaExport = new SchemaExport(cfg);
			await (schemaExport.ExecuteAsync(AddString, false, false, CancellationToken.None));

			string schema = schemaBuilder.ToString();

			Assert.That(schema.Contains("select 'drop script'"), Is.True,
				"schema drop script not exported");

			Assert.That(schema.Contains("select 'create script'"), Is.True,
				"parameterised schema create script not exported");
		} 

	}
}
