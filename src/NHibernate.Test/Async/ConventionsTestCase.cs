#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ConventionsTestCaseAsync
	{
		[Test]
		public void NHibernate_should_be_cls_compliant()
		{
			CLSCompliantAttribute[] attributes = (CLSCompliantAttribute[])typeof (ISession).Assembly.GetCustomAttributes(typeof (CLSCompliantAttribute), true);
			Assert.AreNotEqual(0, attributes.Length, "NHibernate should specify CLS Compliant attribute");
			Assert.IsTrue(attributes[0].IsCompliant, "NHibernate should be CLS Compliant");
		}
	}
}
#endif
