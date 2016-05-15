#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExceptionsTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullQueryTest : TestCase
	{
		[Test]
		public async Task BadGrammarAsync()
		{
			ISession session = OpenSession();
			DbConnection connection = session.Connection;
			try
			{
				DbCommand ps = connection.CreateCommand();
				ps.CommandType = CommandType.Text;
				ps.CommandText = "whatever";
				await (ps.ExecuteNonQueryAsync());
			}
			catch (Exception sqle)
			{
				Assert.DoesNotThrow(() => ADOExceptionHelper.Convert(sessions.SQLExceptionConverter, sqle, "could not get or update next value", null));
			}
			finally
			{
				session.Close();
			}
		}
	}
}
#endif
