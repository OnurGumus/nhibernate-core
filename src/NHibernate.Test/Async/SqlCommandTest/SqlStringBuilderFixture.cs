#if NET_4_5
using System;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlCommandTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlStringBuilderFixtureAsync
	{
		[Test]
		public void InsertAndAdd()
		{
			SqlStringBuilder builder = new SqlStringBuilder();
			builder.Add("col1, col2 ");
			builder.Insert(0, "select ");
			builder.Add("from table ");
			Assert.AreEqual("select col1, col2 from table ", builder.ToSqlString().ToString());
		}

		[Test]
		public void RemoveAt()
		{
			SqlStringBuilder builder = new SqlStringBuilder();
			builder.Add("   select * ");
			builder.Add("from table");
			Assert.AreEqual("   select * from table", builder.ToSqlString().ToString());
			builder.RemoveAt(0);
			Assert.AreEqual("from table", builder.ToSqlString().ToString(), "Removed the first element in the SqlStringBuilder");
			builder.Insert(0, "SELECT * ");
			Assert.AreEqual("SELECT * from table", builder.ToSqlString().ToString());
		}

		[Test]
		public void Index()
		{
			SqlStringBuilder builder = new SqlStringBuilder();
			builder.Add("   select * ");
			builder.Add("from table");
			builder[0] = "SELECT * ";
			Assert.AreEqual("SELECT * from table", builder.ToSqlString().ToString());
			Assert.AreEqual("from table", builder[1]);
		}
	}
}
#endif
