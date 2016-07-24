#if NET_4_5
using NHibernate.AdoNet.Util;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicFormatterFixtureAsync
	{
		[Test]
		public Task StringWithNestedDelimitersAsync()
		{
			try
			{
				string formattedSql = null;
				IFormatter formatter = new BasicFormatter();
				string sql = @"INSERT INTO Table (Name, id) VALUES (@p0, @p1); @p0 = 'a'(b', @p1 = 1";
				Assert.DoesNotThrow(() => formattedSql = formatter.Format(sql));
				Assert.That(formattedSql, Is.StringContaining("'a'(b'"));
				sql = @"UPDATE Table SET Column = @p0;@p0 = '(')'";
				Assert.DoesNotThrow(() => formattedSql = formatter.Format(sql));
				Assert.That(formattedSql, Is.StringContaining("'(')'"));
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
