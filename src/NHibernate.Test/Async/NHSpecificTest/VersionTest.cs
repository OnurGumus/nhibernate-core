#if NET_4_5
using System;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class VersionTestAsync
	{
		[Test]
		public void UnsavedNegativeIntOrShort()
		{
			VersionValue negative = VersionValue.VersionNegative;
			Assert.AreEqual(true, negative.IsUnsaved((short)-1));
			Assert.AreEqual(true, negative.IsUnsaved(-1));
			Assert.AreEqual(true, negative.IsUnsaved(-1L));
		}
	}
}
#endif
