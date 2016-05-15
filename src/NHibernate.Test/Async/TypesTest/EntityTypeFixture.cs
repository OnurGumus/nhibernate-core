#if NET_4_5
using NHibernate.Engine;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task CompareAsync()
		{
			EntityType type = (EntityType)NHibernateUtil.Entity(typeof (EntityClass));
			EntityClass a = new EntityClass(1);
			EntityClass b = new EntityClass(2);
			EntityClass ca = new ComparableEntityClass(1);
			EntityClass cb = new ComparableEntityClass(2);
			Assert.AreEqual(-1, await (type.CompareAsync(a, cb, EntityMode.Poco)));
			Assert.AreEqual(-1, await (type.CompareAsync(ca, b, EntityMode.Poco)));
			Assert.AreEqual(-1, await (type.CompareAsync(ca, cb, EntityMode.Poco)));
			Assert.AreEqual(1, await (type.CompareAsync(b, ca, EntityMode.Poco)));
			Assert.AreEqual(1, await (type.CompareAsync(cb, a, EntityMode.Poco)));
			Assert.AreEqual(1, await (type.CompareAsync(cb, ca, EntityMode.Poco)));
			Assert.AreEqual(0, await (type.CompareAsync(ca, a, EntityMode.Poco)));
			Assert.AreEqual(0, await (type.CompareAsync(a, ca, EntityMode.Poco)));
		}

		[Test]
		public async Task EqualsAsync()
		{
			EntityType type = (EntityType)NHibernateUtil.Entity(typeof (EntityClass));
			EntityClass a = new EntityClass(1);
			EntityClass b = new EntityClass(2);
			EntityClass c = new EntityClass(1);
			Assert.IsTrue(await (type.IsEqualAsync(a, a, EntityMode.Poco, (ISessionFactoryImplementor)sessions)));
			Assert.IsFalse(await (type.IsEqualAsync(a, b, EntityMode.Poco, (ISessionFactoryImplementor)sessions)));
			Assert.IsTrue(await (type.IsEqualAsync(a, c, EntityMode.Poco, (ISessionFactoryImplementor)sessions)));
		}
	}
}
#endif
