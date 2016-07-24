#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableFixtureAsync
	{
		[Test]
		public void TableNameQuoted()
		{
			Table tbl = new Table();
			tbl.Name = "`keyword`";
			Dialect.Dialect dialect = new MsSql2000Dialect();
			Assert.AreEqual("[keyword]", tbl.GetQuotedName(dialect));
			Assert.AreEqual("dbo.[keyword]", tbl.GetQualifiedName(dialect, null, "dbo"));
			Assert.AreEqual("[keyword]", tbl.GetQualifiedName(dialect, null, null));
			tbl.Schema = "sch";
			Assert.AreEqual("sch.[keyword]", tbl.GetQualifiedName(dialect));
		}

		[Test]
		public void TableNameNotQuoted()
		{
			Table tbl = new Table();
			tbl.Name = "notkeyword";
			Dialect.Dialect dialect = new MsSql2000Dialect();
			Assert.AreEqual("notkeyword", tbl.GetQuotedName(dialect));
			Assert.AreEqual("dbo.notkeyword", tbl.GetQualifiedName(dialect, null, "dbo"));
			Assert.AreEqual("notkeyword", tbl.GetQualifiedName(dialect, null, null));
			tbl.Schema = "sch";
			Assert.AreEqual("sch.notkeyword", tbl.GetQualifiedName(dialect));
		}

		[Test]
		public void SchemaNameQuoted()
		{
			Table tbl = new Table();
			tbl.Schema = "`schema`";
			tbl.Name = "name";
			Dialect.Dialect dialect = new MsSql2000Dialect();
			Assert.AreEqual("[schema].name", tbl.GetQualifiedName(dialect));
		}
	}
}
#endif
