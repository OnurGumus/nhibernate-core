#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Dialect;
using NHibernate.Id;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CfgTest.Loquacious
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TypeDefinitionFixtureAsync
	{
		[Test]
		public void AddTypeDef()
		{
			var configure = new Configuration().DataBaseIntegration(db => db.Dialect<MsSql2005Dialect>());
			configure.TypeDefinition<TableHiLoGenerator>(c =>
			{
				c.Alias = "HighLow";
				c.Properties = new
				{
				max_lo = 99
				}

				;
			}

			);
			var mappings = configure.CreateMappings(Dialect.Dialect.GetDialect(configure.Properties));
			var typeDef = mappings.GetTypeDef("HighLow");
			Assert.That(typeDef, Is.Not.Null);
			Assert.That(typeDef.Parameters["max_lo"], Is.EqualTo("99"));
		}
	}
}
#endif
