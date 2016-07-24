#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Generatedkeys.Select
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SelectGeneratorTestAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"Generatedkeys.Select.MyEntity.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.FirebirdDialect || dialect is Dialect.Oracle8iDialect;
		}

		[Test]
		public async Task GetGeneratedKeysSupportAsync()
		{
			ISession session = OpenSession();
			session.BeginTransaction();
			MyEntity e = new MyEntity("entity-1");
			await (session.SaveAsync(e));
			// this insert should happen immediately!
			Assert.AreEqual(1, e.Id, "id not generated through forced insertion");
			await (session.DeleteAsync(e));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
