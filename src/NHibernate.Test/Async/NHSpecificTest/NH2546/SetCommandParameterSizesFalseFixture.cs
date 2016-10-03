#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2546
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SetCommandParameterSizesFalseFixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.MsSql2008Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				await (session.PersistAsync(new Student()
				{StringTypeWithLengthDefined = "Julian Maughan"}));
				await (session.PersistAsync(new Student()
				{StringTypeWithLengthDefined = "Bill Clinton"}));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				await (session.CreateQuery("delete from Student").ExecuteUpdateAsync());
				await (session.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task LikeExpressionWithinDefinedTypeSizeAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria<Student>().Add(Restrictions.Like("StringTypeWithLengthDefined", "Julian%"));
				IList<Student> list = await (criteria.ListAsync<Student>());
				Assert.That(list.Count, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task LikeExpressionExceedsDefinedTypeSizeAsync()
		{
			// In this case we are forcing the usage of LikeExpression class where the length of the associated property is ignored
			using (ISession session = Sfi.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria<Student>().Add(Restrictions.Like("StringTypeWithLengthDefined", "[a-z][a-z][a-z]ian%", MatchMode.Exact, null));
				IList<Student> list = await (criteria.ListAsync<Student>());
				Assert.That(list.Count, Is.EqualTo(1));
			}
		}
	}
}
#endif
