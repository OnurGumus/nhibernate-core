#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2242
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FormulaTestAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect as MsSql2005Dialect != null;
		}

		[Test]
		public async Task FormulaOfEscapedDomainClassShouldBeRetrievedCorrectlyAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var entity = new EscapedFormulaDomainClass();
					entity.Id = 1;
					await (session.SaveAsync(entity));
					await (transaction.CommitAsync());
				}

				session.Clear();
				using (ITransaction transaction = session.BeginTransaction())
				{
					var entity = await (session.GetAsync<EscapedFormulaDomainClass>(1));
					Assert.AreEqual(1, entity.Formula);
					await (session.DeleteAsync(entity));
					await (transaction.CommitAsync());
				}
			}
		}

		[Test]
		public async Task FormulaOfUnescapedDomainClassShouldBeRetrievedCorrectlyAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var entity = new UnescapedFormulaDomainClass();
					entity.Id = 1;
					await (session.SaveAsync(entity));
					await (transaction.CommitAsync());
				}

				session.Clear();
				using (ITransaction transaction = session.BeginTransaction())
				{
					var entity = await (session.GetAsync<UnescapedFormulaDomainClass>(1));
					Assert.AreEqual(1, entity.Formula);
					await (session.DeleteAsync(entity));
					await (transaction.CommitAsync());
				}
			}
		}
	}
}
#endif
