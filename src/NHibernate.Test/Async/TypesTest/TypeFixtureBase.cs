#if NET_4_5
using System;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class TypeFixtureBaseAsync : TestCaseAsync
	{
		protected abstract string TypeName
		{
			get;
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{String.Format("TypesTest.{0}Class.hbm.xml", TypeName)};
			}
		}
	}
}
#endif
