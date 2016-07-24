#if NET_4_5
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2530
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
			{
				configuration.AddAuxiliaryDatabaseObject(CreateHighLowScript(new[]{typeof (Product)}));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		private IAuxiliaryDatabaseObject CreateHighLowScript(IEnumerable<System.Type> entities)
		{
			var script = new StringBuilder(1024);
			script.AppendLine("DELETE FROM NextHighVaues;");
			script.AppendLine("ALTER TABLE NextHighVaues ADD Entity VARCHAR(128) NOT NULL;");
			script.AppendLine("CREATE NONCLUSTERED INDEX IdxNextHighVauesEntity ON NextHighVaues (Entity ASC);");
			script.AppendLine("GO");
			foreach (var entity in entities)
			{
				script.AppendLine(string.Format("INSERT INTO [NextHighVaues] (Entity, NextHigh) VALUES ('{0}',1);", entity.Name));
			}

			var dialects = new HashSet<string>{typeof (MsSql2000Dialect).FullName, typeof (MsSql2005Dialect).FullName, typeof (MsSql2008Dialect).FullName, typeof (MsSql2012Dialect).FullName};
			return new SimpleAuxiliaryDatabaseObject(script.ToString(), null, dialects);
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return (dialect is MsSql2000Dialect);
		}

		[Test]
		public async Task WhenTryToGetHighThenExceptionShouldContainWhereClauseAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var customer = new Customer{Name = "Mengano"};
					Assert.That(async () => await (session.PersistAsync(customer)), Throws.Exception.Message.ContainsSubstring("Entity = 'Customer'"));
				}
		}
	}
}
#endif
