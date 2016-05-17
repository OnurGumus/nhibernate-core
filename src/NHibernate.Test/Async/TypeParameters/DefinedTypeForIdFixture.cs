#if NET_4_5
using System.Collections;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypeParameters
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefinedTypeForIdFixture : TestCase
	{
		[Test]
		[Description("Ensure the parametrized generator is working.")]
		public async Task SaveAsync()
		{
			object savedId1;
			object savedId2;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					savedId1 = await (s.SaveAsync(new EntityCustomId()));
					savedId2 = await (s.SaveAsync(new EntityCustomId()));
					await (t.CommitAsync());
				}

			Assert.That(savedId1, Is.LessThan(200), "should be work with custo parameters");
			Assert.That(savedId1, Is.GreaterThan(99));
			Assert.That(savedId2, Is.EqualTo((int)savedId1 + 1));
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.CreateQuery("delete from EntityCustomId").ExecuteUpdate();
					await (t.CommitAsync());
				}
		}
	}
}
#endif
