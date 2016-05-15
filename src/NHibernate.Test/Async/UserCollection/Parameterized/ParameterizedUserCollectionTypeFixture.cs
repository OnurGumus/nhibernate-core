#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UserCollection.Parameterized
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ParameterizedUserCollectionTypeFixture : TestCase
	{
		[Test]
		public async Task BasicOperationAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var entity = new Entity("tester");
					entity.Values.Add("value-1");
					await (s.PersistAsync(entity));
					await (t.CommitAsync());
				}

			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var entity = await (s.GetAsync<Entity>("tester"));
					Assert.IsTrue(NHibernateUtil.IsInitialized(entity.Values));
					Assert.AreEqual(1, entity.Values.Count);
					Assert.AreEqual("Hello", ((IDefaultableList)entity.Values).DefaultValue);
					await (s.DeleteAsync(entity));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
