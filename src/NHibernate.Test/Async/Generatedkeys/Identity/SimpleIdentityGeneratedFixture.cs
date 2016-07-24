#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Generatedkeys.Identity
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleIdentityGeneratedFixtureAsync : TestCaseAsync
	{
		// This test is to check the support of identity generator
		// NH should choose one of the identity-style generation where the Dialect are supporting one of them
		// as identity, sequence-identity (identity.sequence), generated (identity.sequence)
		protected override IList Mappings
		{
			get
			{
				return new[]{"Generatedkeys.Identity.MyEntityIdentity.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test]
		public async Task SequenceIdentityGeneratorAsync()
		{
			ISession session = OpenSession();
			session.BeginTransaction();
			var e = new MyEntityIdentity{Name = "entity-1"};
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
