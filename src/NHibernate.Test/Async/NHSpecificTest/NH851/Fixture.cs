#if NET_4_5
using System;
using NHibernate.Type;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH851
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class SomeClass
		{
			public SomeClass(int x)
			{
			}
		}

		[Test]
		public void ConstructorNotFound()
		{
			try
			{
				ReflectHelper.GetConstructor(typeof (SomeClass), new IType[]{NHibernateUtil.String});
				Assert.Fail("Should have thrown an exception");
			}
			catch (InstantiationException e)
			{
				Assert.IsTrue(e.Message.IndexOf("String") >= 0);
				Assert.IsTrue(e.Message.IndexOf(typeof (SomeClass).ToString()) >= 0);
			}
		}
	}
}
#endif
