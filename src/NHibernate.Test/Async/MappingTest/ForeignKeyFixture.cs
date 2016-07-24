#if NET_4_5
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.MappingTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ForeignKeyFixtureAsync
	{
		[Test]
		public Task UnmatchingColumnsAsync()
		{
			try
			{
				Table primaryTable = new Table("pktable");
				primaryTable.PrimaryKey = new PrimaryKey();
				SimpleValue sv = new SimpleValue();
				sv.TypeName = NHibernateUtil.Int16.Name;
				Column pkColumn = new Column("pk_column");
				pkColumn.Value = sv;
				primaryTable.PrimaryKey.AddColumn(pkColumn);
				Table fkTable = new Table("fktable");
				ForeignKey fk = new ForeignKey();
				sv = new SimpleValue();
				sv.TypeName = NHibernateUtil.Int16.Name;
				Column fkColumn1 = new Column("col1");
				fkColumn1.Value = sv;
				sv = new SimpleValue();
				sv.TypeName = NHibernateUtil.Int16.Name;
				Column fkColumn2 = new Column("col2");
				fkColumn2.Value = sv;
				fk.AddColumn(fkColumn1);
				fk.AddColumn(fkColumn2);
				fk.Table = fkTable;
				fk.ReferencedTable = primaryTable;
				Assert.Throws<FKUnmatchingColumnsException>(() => fk.AlignColumns());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task ToStringDoesNotThrowAsync()
		{
			try
			{
				var key = new ForeignKey{Table = new Table("TestTable"), Name = "TestForeignKey"};
				key.AddColumn(new Column("TestColumn"));
				key.AddReferencedColumns(new[]{new Column("ReferencedColumn")});
				string toString = null;
				Assert.DoesNotThrow(() =>
				{
					toString = key.ToString();
				}

				);
				Assert.That(toString, Is.EqualTo("NHibernate.Mapping.ForeignKey(TestTableNHibernate.Mapping.Column(TestColumn) ref-columns:(NHibernate.Mapping.Column(ReferencedColumn)) as TestForeignKey"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
