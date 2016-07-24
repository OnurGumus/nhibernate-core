#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Hql
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlCommentsFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Hql.Animal.hbm.xml"};
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.SetProperty(Environment.UseSqlComments, "true");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task CommentsInQueryAsync()
		{
			using (ISession s = OpenSession())
			{
				using (var sl = new SqlLogSpy())
				{
					await (s.CreateQuery("from Animal").SetComment("This is my query").ListAsync());
					string sql = sl.Appender.GetEvents()[0].RenderedMessage;
					Assert.That(sql.IndexOf("This is my query"), Is.GreaterThan(0));
				}
			}
		}
	}
}
#endif
