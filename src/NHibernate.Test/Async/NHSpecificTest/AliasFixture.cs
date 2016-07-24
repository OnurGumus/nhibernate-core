#if NET_4_5
using System;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AliasFixtureAsync
	{
		[Test]
		public void NoLeadingUnderscores()
		{
			Alias alias = new Alias("suffix");
			Dialect.Dialect dialect = new MsSql2000Dialect();
			Assert.IsFalse(alias.ToAliasString("__someIdentifier", dialect).StartsWith("_"));
			Assert.IsFalse(alias.ToUnquotedAliasString("__someIdentifier", dialect).StartsWith("_"));
		}
	}
}
#endif
