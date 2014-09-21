using NHibernate.Test.Cascade.Circle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			var t = new CascadeMergeToChildBeforeParentTest();
			t.TestFixtureSetUp();
			t.SetUp();
			t.MergeData3Nodes();
		}
	}
}
