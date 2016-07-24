#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Dialect.Function;
using System.Reflection;
using System.Threading.Tasks;

namespace NHibernate.Test.DialectTest.FunctionTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SerializableTypesFixtureAsync
	{
		[Test]
		public void AllEmbeddedTypesAreMarkedSerializable()
		{
			NHAssert.InheritedAreMarkedSerializable(typeof (ISQLFunction));
		}
	}
}
#endif
