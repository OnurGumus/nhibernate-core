#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1710
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class BaseFixture
	{
		[Test]
		public async Task NotIgnorePrecisionScaleInSchemaExportAsync()
		{
			var script = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => script.AppendLine(sl), true));
			Assert.That(script.ToString(), Is.StringContaining(expectedExportString));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithExplicitDefinedTypeAsync : BaseFixture
	{
		protected override string GetResourceName()
		{
			return "Heuristic.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithHeuristicDefinedTypeAsync : BaseFixture
	{
		protected override string GetResourceName()
		{
			return "Defined.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithInLineDefinedTypeAsync : BaseFixture
	{
		protected override string GetResourceName()
		{
			return "InLine.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithColumnNodeAsync : BaseFixture
	{
		protected override string GetResourceName()
		{
			return "WithColumnNode.hbm.xml";
		}
	}
}
#endif
