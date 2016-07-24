#if NET_4_5
using System.Collections.Generic;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1515
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private readonly IProxyValidator pv = new DynProxyTypeValidator();
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ClassWithInternal
		{
			internal virtual void DoSomething()
			{
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ClassWithInternalProperty
		{
			internal virtual string DoSomething
			{
				get;
				set;
			}
		}

		[Test]
		public void NoExceptionForMethod()
		{
			ICollection<string> errs = pv.ValidateType(typeof (ClassWithInternal));
			Assert.That(errs, Is.Not.Null);
			Assert.That(errs.Count, Is.EqualTo(1));
		}

		[Test]
		public void NoExceptionForProperty()
		{
			ICollection<string> errs = pv.ValidateType(typeof (ClassWithInternalProperty));
			Assert.That(errs, Is.Not.Null);
			Assert.That(errs.Count, Is.EqualTo(2));
		}
	}
}
#endif
