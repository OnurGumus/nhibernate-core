#if NET_4_5
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LinqReadonlyTestsContext
	{
		[SetUp]
		public async Task CreateNorthwindDbAsync()
		{
			Configuration configuration = Configure();
			string scripFileName = GetScripFileName(configuration, "LinqReadonlyCreateScript");
			if (File.Exists(scripFileName))
			{
				ExecuteScriptFile(configuration, scripFileName);
			}
			else
			{
				// may crash with NUnit2.5+ test runner
				new SchemaExport(configuration).Create(false, true);
				ISessionFactory sessionFactory = configuration.BuildSessionFactory();
				await (CreateTestDataAsync(sessionFactory));
			}
		}

		private async Task CreateTestDataAsync(ISessionFactory sessionFactory)
		{
			using (IStatelessSession session = sessionFactory.OpenStatelessSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					NorthwindDbCreator.CreateNorthwindData(session);
					await (tx.CommitAsync());
				}

			using (ISession session = sessionFactory.OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (NorthwindDbCreator.CreateMiscTestDataAsync(session));
					await (NorthwindDbCreator.CreatePatientDataAsync(session));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
