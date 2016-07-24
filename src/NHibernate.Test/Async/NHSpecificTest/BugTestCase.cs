#if NET_4_5
using System;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class BugTestCaseAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		public virtual string BugNumber
		{
			get
			{
				string ns = GetType().Namespace;
				return ns.Substring(ns.LastIndexOf('.') + 1);
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest." + BugNumber + ".Mappings.hbm.xml"};
			}
		}
	}
}
#endif
