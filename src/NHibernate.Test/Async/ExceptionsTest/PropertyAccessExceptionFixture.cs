#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExceptionsTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyAccessExceptionFixtureAsync
	{
		/// <summary>
		/// Verifying that NH-358 has been fixed.
		/// </summary>
		[Test]
		public void MessageWithoutTypeCtor()
		{
			PropertyAccessException exc = new PropertyAccessException(null, "notype", true, null, "PropName");
			Assert.AreEqual("notype setter of UnknownType.PropName", exc.Message);
		}
	}
}
#endif
