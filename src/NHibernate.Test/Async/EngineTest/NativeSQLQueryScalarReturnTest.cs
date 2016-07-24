#if NET_4_5
using NHibernate.Engine.Query.Sql;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.EngineTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSQLQueryScalarReturnTestAsync
	{
		[Test]
		public void EqualsByAlias()
		{
			var sr1 = new NativeSQLQueryScalarReturn("myAlias", NHibernateUtil.Int32);
			var sr2 = new NativeSQLQueryScalarReturn("myAlias", NHibernateUtil.Int32);
			Assert.AreEqual(sr1, sr2);
		}

		[Test]
		public void HashCodeByAlias()
		{
			var sr1 = new NativeSQLQueryScalarReturn("myAlias", NHibernateUtil.Int32);
			var sr2 = new NativeSQLQueryScalarReturn("myAlias", NHibernateUtil.Int32);
			Assert.AreEqual(sr1.GetHashCode(), sr2.GetHashCode());
		}
	}
}
#endif
