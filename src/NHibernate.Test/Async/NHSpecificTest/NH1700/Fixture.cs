﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1700
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync
	{
		[Test]
		public async Task ShouldNotThrowDuplicateMappingAsync()
		{
			var cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);

			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1700.Mappings.hbm.xml", GetType().Assembly);
			await (new SchemaExport(cfg).CreateAsync(false, true, CancellationToken.None));

			await (new SchemaExport(cfg).DropAsync(false, true, CancellationToken.None));
		}
	}
}