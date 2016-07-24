#if NET_4_5
using System;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ReflectionOptimizerTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LcgFixtureAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NoSetterClass
		{
			public int Property
			{
				get
				{
					return 0;
				}
			}
		}

		[Test]
		public Task NoSetterAsync()
		{
			try
			{
				IGetter[] getters = new IGetter[]{new BasicPropertyAccessor.BasicGetter(typeof (NoSetterClass), typeof (NoSetterClass).GetProperty("Property"), "Property")};
				ISetter[] setters = new ISetter[]{new BasicPropertyAccessor.BasicSetter(typeof (NoSetterClass), typeof (NoSetterClass).GetProperty("Property"), "Property")};
				Assert.Throws<PropertyNotFoundException>(() => new ReflectionOptimizer(typeof (NoSetterClass), getters, setters));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NoGetterClass
		{
			public int Property
			{
				set
				{
				}
			}
		}

		[Test]
		public Task NoGetterAsync()
		{
			try
			{
				IGetter[] getters = new IGetter[]{new BasicPropertyAccessor.BasicGetter(typeof (NoGetterClass), typeof (NoGetterClass).GetProperty("Property"), "Property")};
				ISetter[] setters = new ISetter[]{new BasicPropertyAccessor.BasicSetter(typeof (NoGetterClass), typeof (NoGetterClass).GetProperty("Property"), "Property")};
				Assert.Throws<PropertyNotFoundException>(() => new ReflectionOptimizer(typeof (NoGetterClass), getters, setters));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
