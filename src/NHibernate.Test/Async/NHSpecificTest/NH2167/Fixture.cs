#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2167
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task GroupPropertyWithSqlFunctionDoesNotThrowAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var projection = session.CreateCriteria<Entity>().SetProjection(Projections.GroupProperty(Projections.SqlFunction("substring", NHibernateUtil.String, Projections.Property("Name"), Projections.Constant(0), Projections.Constant(1))));
					await (projection.ListAsync());
				}
		}
	}
}
#endif
