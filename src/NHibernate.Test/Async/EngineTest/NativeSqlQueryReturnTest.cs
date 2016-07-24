#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Engine.Query.Sql;
using System.Threading.Tasks;

namespace NHibernate.Test.EngineTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSqlQueryReturnTestAsync
	{
		[Test]
		public void AllEmbeddedTypesAreMarkedSerializable()
		{
			NHAssert.InheritedAreMarkedSerializable(typeof (INativeSQLQueryReturn));
		}
	}
}
#endif
