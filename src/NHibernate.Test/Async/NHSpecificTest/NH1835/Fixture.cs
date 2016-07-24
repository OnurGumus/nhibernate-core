#if NET_4_5
using NUnit.Framework;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1835
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public void ColumnTypeBinaryBlob()
		{
			var pc = sessions.GetEntityPersister(typeof (Document).FullName);
			var type = pc.GetPropertyType("Contents");
			Assert.That(type.SqlTypes(sessions)[0], Is.InstanceOf<BinaryBlobSqlType>());
		}
	}
}
#endif
