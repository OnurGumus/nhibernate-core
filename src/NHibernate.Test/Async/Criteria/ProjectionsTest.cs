#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProjectionsTest : TestCase
	{
		[Test]
		public async Task UsingSqlFunctions_ConcatAsync()
		{
			using (ISession session = sessions.OpenSession())
			{
				string result = await (session.CreateCriteria(typeof (Student)).SetProjection(new SqlFunctionProjection("concat", NHibernateUtil.String, Projections.Property("Name"), new ConstantProjection(" "), Projections.Property("Name"))).UniqueResultAsync<string>());
				Assert.AreEqual("ayende ayende", result);
			}
		}

		[Test]
		public async Task UsingSqlFunctions_Concat_WithCastAsync()
		{
			if (Dialect is Oracle8iDialect)
			{
				Assert.Ignore("Not supported by the active dialect:{0}.", Dialect);
			}

			using (ISession session = sessions.OpenSession())
			{
				string result = await (session.CreateCriteria(typeof (Student)).SetProjection(Projections.SqlFunction("concat", NHibernateUtil.String, Projections.Cast(NHibernateUtil.String, Projections.Id()), Projections.Constant(" "), Projections.Property("Name"))).UniqueResultAsync<string>());
				Assert.AreEqual("27 ayende", result);
			}
		}

		[Test]
		public async Task CanUseParametersWithProjectionsAsync()
		{
			using (ISession session = sessions.OpenSession())
			{
				long result = await (session.CreateCriteria(typeof (Student)).SetProjection(new AddNumberProjection("id", 15)).UniqueResultAsync<long>());
				Assert.AreEqual(42L, result);
			}
		}

		[Test]
		public async Task UsingConditionalsAsync()
		{
			using (ISession session = sessions.OpenSession())
			{
				string result = await (session.CreateCriteria(typeof (Student)).SetProjection(Projections.Conditional(Expression.Eq("id", 27L), Projections.Constant("yes"), Projections.Constant("no"))).UniqueResultAsync<string>());
				Assert.AreEqual("yes", result);
				result = await (session.CreateCriteria(typeof (Student)).SetProjection(Projections.Conditional(Expression.Eq("id", 42L), Projections.Constant("yes"), Projections.Constant("no"))).UniqueResultAsync<string>());
				Assert.AreEqual("no", result);
			}
		}
	}
}
#endif
