#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SByteTypeFixtureAsync
	{
		[Test]
		public void Equals()
		{
			SByteType type = (SByteType)NHibernateUtil.SByte;
			Assert.IsTrue(type.IsEqual((sbyte)-1, (sbyte)-1));
			Assert.IsFalse(type.IsEqual((sbyte)-2, (sbyte)-1));
		}

		[Test]
		public void ObjectToSQLString()
		{
			SByteType type = (SByteType)NHibernateUtil.SByte;
			Assert.AreEqual("-1", type.ObjectToSQLString((sbyte)-1, new MsSql2000Dialect()));
		}

		[Test]
		public void StringToObject()
		{
			SByteType type = (SByteType)NHibernateUtil.SByte;
			Assert.AreEqual((sbyte)-1, type.StringToObject("-1"));
		}
	}
}
#endif
