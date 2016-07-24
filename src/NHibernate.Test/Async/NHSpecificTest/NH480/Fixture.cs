#if NET_4_5
using System;
using System.Globalization;
using System.Threading;
using NHibernate.Cfg;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH480
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private CultureInfo currentCulture = null;
		private CultureInfo currentUICulture = null;
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			currentCulture = Thread.CurrentThread.CurrentCulture;
			currentUICulture = Thread.CurrentThread.CurrentUICulture;
			CultureInfo turkish = new CultureInfo("tr-TR");
			Thread.CurrentThread.CurrentCulture = turkish;
			Thread.CurrentThread.CurrentUICulture = turkish;
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			Thread.CurrentThread.CurrentUICulture = currentUICulture;
		}

		[Test]
		public void CheckIII()
		{
			Assert.AreEqual("iii", new CamelCaseStrategy().GetFieldName("Iii"));
			Assert.AreEqual("_iii", new CamelCaseUnderscoreStrategy().GetFieldName("Iii"));
			Assert.AreEqual("iii", new LowerCaseStrategy().GetFieldName("III"));
			Assert.AreEqual("_iii", new LowerCaseUnderscoreStrategy().GetFieldName("III"));
			Assert.AreEqual("m_Iii", new PascalCaseMUnderscoreStrategy().GetFieldName("iii"));
			Assert.AreEqual("_Iii", new PascalCaseUnderscoreStrategy().GetFieldName("iii"));
			Assert.AreEqual("iii_iii_iii", ImprovedNamingStrategy.Instance.ColumnName("IiiIiiIii"));
		}
	}
}
#endif
