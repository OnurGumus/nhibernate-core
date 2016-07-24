#if NET_4_5
using System.Collections.Generic;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CfgTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SettingsFactoryFixtureAsync
	{
		[Test]
		public void DefaultValueForKeyWords()
		{
			var properties = new Dictionary<string, string>{{"dialect", typeof (Dialect.MsSql2005Dialect).FullName}};
			var settings = new SettingsFactory().BuildSettings(properties);
			Assert.That(settings.IsKeywordsImportEnabled);
			Assert.That(!settings.IsAutoQuoteEnabled);
		}
	}
}
#endif
