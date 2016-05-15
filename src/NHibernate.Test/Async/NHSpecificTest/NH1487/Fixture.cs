#if NET_4_5
using System;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1487
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task GenerateSchemaMultipleUniqueKeysAsync()
		{
			var cfg = GetConf();
			if (!(Dialect.Dialect.GetDialect(cfg.Properties) is MsSql2000Dialect))
			{
				Assert.Ignore("Specific for MsSql2000Dialect");
			}

			const string hbm = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
namespace='NHibernate.Test.NHSpecificTest.NH1487'
assembly='NHibernate.Test'>
    <class name='Entity' >
        <id name='Id' >
            <generator class='assigned' />
        </id>
        <property name='A' unique-key='AC'/>
        <property name='B' unique-key='BC'/>
        <property name='C' unique-key='AC, BC'/>
    </class>
</hibernate-mapping>";
			cfg.AddXmlString(hbm);
			// Can create the schema
			var scriptB = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => scriptB.Append(sl), true));
			var script = scriptB.ToString();
			Assert.That(script, Is.StringContaining("unique (A, C)"));
			Assert.That(script, Is.StringContaining("unique (B, C)"));
			Assert.That(script, Is.Not.StringContaining("unique (C)"));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}

		[Test]
		public async Task GenerateSchemaMultipleIndexAsync()
		{
			var cfg = GetConf();
			if (!(Dialect.Dialect.GetDialect(cfg.Properties) is MsSql2000Dialect))
			{
				Assert.Ignore("Specific for MsSql2000Dialect");
			}

			const string hbm = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
namespace='NHibernate.Test.NHSpecificTest.NH1487'
assembly='NHibernate.Test'>
    <class name='Entity' >
        <id name='Id' >
            <generator class='assigned' />
        </id>
        <property name='A' index='AC'/>
        <property name='B' index='BC'/>
        <property name='C' index='AC, BC'/>
    </class>
</hibernate-mapping>";
			cfg.AddXmlString(hbm);
			var scriptB = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => scriptB.Append(sl), true));
			var script = scriptB.ToString();
			Assert.That(script, Is.StringContaining("create index AC on Entity (A, C)"));
			Assert.That(script, Is.StringContaining("create index BC on Entity (B, C)"));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}

		[Test]
		public async Task GenerateSchemaMultipleIndexOnColumnAsync()
		{
			var cfg = GetConf();
			if (!(Dialect.Dialect.GetDialect(cfg.Properties) is MsSql2000Dialect))
			{
				Assert.Ignore("Specific for MsSql2000Dialect");
			}

			const string hbm = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
namespace='NHibernate.Test.NHSpecificTest.NH1487'
assembly='NHibernate.Test'>
    <class name='Entity' >
        <id name='Id' >
            <generator class='assigned' />
        </id>
        <property name='A'>
           <column name='A' index='AC'/>
        </property>
        <property name='B'>
           <column name='B' index='BC'/>
        </property>
        <property name='C'>
           <column name='C' index='AC,BC'/>
        </property>
    </class>
</hibernate-mapping>";
			cfg.AddXmlString(hbm);
			var scriptB = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => scriptB.Append(sl), true));
			var script = scriptB.ToString();
			Assert.That(script, Is.StringContaining("create index AC on Entity (A, C)"));
			Assert.That(script, Is.StringContaining("create index BC on Entity (B, C)"));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}

		[Test]
		public async Task GenerateSchemaIndexOnIdAsync()
		{
			var cfg = GetConf();
			if (!(Dialect.Dialect.GetDialect(cfg.Properties) is MsSql2000Dialect))
			{
				Assert.Ignore("Specific for MsSql2000Dialect");
			}

			const string hbm = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
namespace='NHibernate.Test.NHSpecificTest.NH1487'
assembly='NHibernate.Test'>
    <class name='Entity' >
        <id name='Id' >
            <column name='Id' index='IdxId1,IdxId2'/>
            <generator class='assigned' />
        </id>
        <property name='A'/>
        <property name='B'/>
        <property name='C'/>
    </class>
</hibernate-mapping>";
			cfg.AddXmlString(hbm);
			var scriptB = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => scriptB.Append(sl), true));
			var script = scriptB.ToString();
			Assert.That(script, Is.StringContaining("create index IdxId1 on Entity (Id)"));
			Assert.That(script, Is.StringContaining("create index IdxId2 on Entity (Id)"));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}

		[Test]
		public async Task GenerateSchemaUniqueOnIdAsync()
		{
			var cfg = GetConf();
			if (!(Dialect.Dialect.GetDialect(cfg.Properties) is MsSql2000Dialect))
			{
				Assert.Ignore("Specific for MsSql2000Dialect");
			}

			const string hbm = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
namespace='NHibernate.Test.NHSpecificTest.NH1487'
assembly='NHibernate.Test'>
    <class name='Entity' >
        <id name='Id' >
            <column name='Id' unique-key='UIdxId1,UIdxId2'/>
            <generator class='assigned' />
        </id>
        <property name='A'/>
        <property name='B'/>
        <property name='C'/>
    </class>
</hibernate-mapping>";
			cfg.AddXmlString(hbm);
			var scriptB = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => scriptB.AppendLine(sl), true));
			var script = scriptB.ToString().Split(new[]{System.Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);
			int count = 0;
			foreach (var s in script)
			{
				if (s.Contains("unique (Id)"))
					count++;
			}

			Assert.That(count, Is.EqualTo(2));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}
	}
}
#endif
