#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithExplicitDefinedTypeAsync : TypeSqlTypeFixture
	{
		protected override string GetResourceName()
		{
			return "MultiTypeEntity_Defined.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithHeuristicDefinedTypeAsync : TypeSqlTypeFixture
	{
		protected override string GetResourceName()
		{
			return "MultiTypeEntity_Heuristic.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithInLineDefinedTypeAsync : TypeSqlTypeFixture
	{
		protected override string GetResourceName()
		{
			return "MultiTypeEntity_InLine.hbm.xml";
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithColumnNodeAsync : TypeSqlTypeFixture
	{
		protected override string GetResourceName()
		{
			return "MultiTypeEntity_WithColumnNode.hbm.xml";
		}
	}

	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithSqlTypeAsync : TypeSqlTypeFixture
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		protected override string GetResourceName()
		{
			return "MultiTypeEntity_WithSqlType.hbm.xml";
		}
	}
}
#endif
