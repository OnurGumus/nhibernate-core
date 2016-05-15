#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1274ExportExclude
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1274ExportExcludeFixture
	{
		[Test]
		public async Task SchemaExport_Drop_CreatesDropScriptAsync()
		{
			Configuration configuration = GetConfiguration();
			SchemaExport export = new SchemaExport(configuration);
			TextWriter tw = new StringWriter();
			await (export.DropAsync(tw, false));
			string s = tw.ToString();
			var dialect = Dialect.Dialect.GetDialect(configuration.Properties);
			if (dialect.SupportsIfExistsBeforeTableName)
			{
				Assert.IsTrue(s.Contains("drop table if exists Home_Drop"));
				Assert.IsTrue(s.Contains("drop table if exists Home_All"));
			}
			else
			{
				Assert.IsTrue(s.Contains("drop table Home_Drop"));
				Assert.IsTrue(s.Contains("drop table Home_All"));
			}
		}

		[Test]
		public async Task SchemaExport_Export_CreatesExportScriptAsync()
		{
			Configuration configuration = GetConfiguration();
			SchemaExport export = new SchemaExport(configuration);
			TextWriter tw = new StringWriter();
			await (export.CreateAsync(tw, false));
			string s = tw.ToString();
			var dialect = Dialect.Dialect.GetDialect(configuration.Properties);
			if (dialect.SupportsIfExistsBeforeTableName)
			{
				Assert.IsTrue(s.Contains("drop table if exists Home_Drop"));
				Assert.IsTrue(s.Contains("drop table if exists Home_All"));
			}
			else
			{
				Assert.IsTrue(s.Contains("drop table Home_Drop"));
				Assert.IsTrue(s.Contains("drop table Home_All"));
			}

			Assert.IsTrue(s.Contains("create table Home_All"));
			Assert.IsTrue(s.Contains("create table Home_Export"));
		}

		[Test]
		public async Task SchemaExport_Update_CreatesUpdateScriptAsync()
		{
			Configuration configuration = GetConfiguration();
			SchemaUpdate update = new SchemaUpdate(configuration);
			TextWriter tw = new StringWriter();
			await (update.ExecuteAsync(tw.WriteLine, false));
			string s = tw.ToString();
			Assert.IsTrue(s.Contains("create table Home_Update"));
			Assert.IsTrue(s.Contains("create table Home_All"));
		}

		[Test]
		public async Task SchemaExport_Validate_CausesValidateExceptionAsync()
		{
			Configuration configuration = GetConfiguration();
			SchemaValidator validator = new SchemaValidator(configuration);
			try
			{
				await (validator.ValidateAsync());
			}
			catch (HibernateException he)
			{
				Assert.IsTrue(he.Message.Contains("Home_Validate"));
				return;
			}

			throw new Exception("Should not get to this exception");
		}
	}
}
#endif
