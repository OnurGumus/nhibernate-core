#if NET_4_5
using System;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1464
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private readonly IProxyValidator pv = new DynProxyTypeValidator();
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class CPPMimicBase
		{
			public virtual void Dispose()
			{
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class CPPMimic : CPPMimicBase
		{
			public sealed override void Dispose()
			{
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class Another : IDisposable
		{
			protected void Dispose(bool disposing)
			{
			}

			public void Dispose()
			{
			}

			~Another()
			{
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class OneMore : IDisposable
		{
			public void Dispose(bool disposing)
			{
			}

			public void Dispose()
			{
			}

			~OneMore()
			{
			}
		}

		[Test]
		public void NoExceptionForDispose()
		{
			ICollection<string> errs = pv.ValidateType(typeof (CPPMimic));
			Assert.That(errs, Is.Null);
			errs = pv.ValidateType(typeof (Another));
			Assert.That(errs, Is.Null);
			errs = pv.ValidateType(typeof (OneMore));
			Assert.That(errs.Count, Is.EqualTo(1));
		}
	}
}
#endif
