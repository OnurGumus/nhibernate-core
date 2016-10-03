#if NET_4_5
using System;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class IdFixtureBaseAsync : TestCaseAsync
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
				return new string[]{String.Format("IdTest.{0}Class.hbm.xml", TypeName)};
			}
		}
	}
}
#endif
